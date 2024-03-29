﻿using Proof.Core.Logging;
using Proof.Core.Text;
using Proof.Render;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class TextComponent : IComponent
    {
        private readonly Font _font;
        private readonly IRenderer _renderer;

        public TextComponent(
            string text,
            Font font,
            IRenderer renderer,
            Model model,
            int layer,
            IShader shader)
        {
            _font = font;
            _renderer = renderer;

            Text = text;
            Shader = shader;
            Layer = layer;
            Model = model;
        }

        public string Text { get; set; }

        // Model must be a rectangle
        public Model Model { get; private set; }

        public int Layer { get; set; }

        public IShader Shader { get; set; }

        public void Update(Entity entity)
        {
            TransformComponent? transform = entity.GetComponent<TransformComponent>();
            Vector2 position = transform?.Position ?? Vector2.Zero;
            Vector2 scale = transform?.Scale ?? Vector2.One;

            foreach (char c in Text)
            {
                if(c == '\n')
                {
                    position.X = transform?.Position.X ?? 0;
                    position.Y -= _font.LineHeight / (float)_font.TextureHeight;
                    continue;
                }

                float[] verticesBuffer = new float[Model.Vertices.Length];
                Array.Copy(Model.Vertices, verticesBuffer, verticesBuffer.Length);

                FontCharacter fontCharacter = _font.GetCharacterInformation(c);
                UpdateTransform(verticesBuffer, position, scale, fontCharacter);
                UpdateTextureCoords(verticesBuffer, fontCharacter);

                _renderer.Submit(verticesBuffer, Model.Indices, Layer, _font.Texture, Shader);

                position.X += fontCharacter.XAdvance / (float)_font.TextureWidth * scale.X;
            }
        }

        private void UpdateTransform(float[] verticesBuffer, Vector2 position, Vector2 scale, FontCharacter fontCharacter)
        {
            var layout = Shader.GetLayout();
            if (layout == null)
            {
                throw new InvalidOperationException("Vertex Layout can not be null");
            }

            float lineAdjust = _font.Base / (float)_font.TextureHeight * scale.Y;
            float xScale = fontCharacter.Width / (float)_font.TextureWidth * scale.X;
            float yScale = fontCharacter.Height / (float)_font.TextureHeight * scale.Y;
            float xOffset = fontCharacter.XOffset / (float)_font.TextureWidth * scale.X;
            float yOffset = (-fontCharacter.Height-fontCharacter.YOffset) / (float)_font.TextureHeight * scale.Y;

            for (int i = 0; i < verticesBuffer.Length; i += layout.SumOfElements())
            {
                verticesBuffer[i + layout.PositionIndex] = verticesBuffer[i + layout.PositionIndex] * xScale + position.X + xOffset;
                verticesBuffer[i + layout.PositionIndex + 1] = verticesBuffer[i + layout.PositionIndex + 1] * yScale + yOffset + position.Y + lineAdjust;
            }
        }

        private void UpdateTextureCoords(float[] verticesBuffer, FontCharacter fontCharacter)
        {
            var layout = Shader.GetLayout();
            if(layout == null)
            {
                throw new InvalidOperationException("Vertex Layout can not be null");
            }

            if(layout.TextureCoordsIndex == null)
            {
                throw new InvalidOperationException("Texture coords index must be set for shader used by TextComponent");
            }

            int textureCoordsIndex = layout.TextureCoordsIndex.Value;

            var positions = new List<Vector2>();
            for(int i = 0; i < verticesBuffer.Length; i += layout.SumOfElements())
            {
                positions.Add(new Vector2(verticesBuffer[layout.PositionIndex + i], verticesBuffer[layout.PositionIndex + i + 1]));
            }

            Vector2 bottomLeft = new Vector2(positions.Min(x => x.X), positions.Min(x => x.Y));
            Vector2 topRight = new Vector2(positions.Max(x => x.X), positions.Max(x => x.Y));

            // The coordinates from the font character need updating to OpenGL
            float fontX = fontCharacter.X / (float)_font.TextureWidth;
            float fontY = 1 - fontCharacter.Y / (float)_font.TextureHeight;
            float fontWidth = fontCharacter.Width / (float)_font.TextureWidth;
            float fontHeight = fontCharacter.Height / (float)_font.TextureHeight;

            for(int i = 0; i < verticesBuffer.Length; i += layout.SumOfElements())
            {
                float x = verticesBuffer[layout.PositionIndex + i];
                float y = verticesBuffer[layout.PositionIndex + i + 1];

                if(x == bottomLeft.X && y == bottomLeft.Y)
                {
                    verticesBuffer[i + textureCoordsIndex] = fontX;
                    verticesBuffer[i + textureCoordsIndex + 1] = fontY - fontHeight;
                }
                else if(x == bottomLeft.X && y == topRight.Y)
                {
                    verticesBuffer[i + textureCoordsIndex] = fontX;
                    verticesBuffer[i + textureCoordsIndex + 1] = fontY;
                }
                else if(x == topRight.X && y == bottomLeft.Y)
                {
                    verticesBuffer[i + textureCoordsIndex] = fontX + fontWidth;
                    verticesBuffer[i + textureCoordsIndex + 1] = fontY - fontHeight;
                }
                else if(x == topRight.X && y == topRight.Y)
                {
                    verticesBuffer[i + textureCoordsIndex] = fontX + fontWidth;
                    verticesBuffer[i + textureCoordsIndex + 1] = fontY;
                }
            }
        }

        public XElement ToXml()
        {
            return new XElement(
                "TextComponent",
                new XElement("Layer", Layer),
                new XElement("Model", Model.Path),
                new XElement("Shader", Shader.FilePath),
                new XElement("Text", Text),
                new XElement("Font", _font.FilePath));
        }

        public static IComponent LoadFromNode(
            ALogger logger,
            FontLibrary fontLibrary,
            ModelLibrary modelLibrary,
            ShaderLibrary shaderLibrary,
            IRenderer renderer,
            XElement componentNode)
        {
            XElement? textElement = componentNode.Element("Text");
            if(textElement == null)
            {
                logger.LogWarn("Could not find Text node while loading TextComponent. Assuming empty string");
            }

            string text = textElement?.Value ?? string.Empty;

            XElement? fontNode = componentNode.Element("Font");
            if (fontNode == null)
            {
                throw new XmlException("Could not find Font node while loading TextComponent.");
            }

            Font font = fontLibrary.Get(fontNode.Value);

            XElement? modelNode = componentNode.Element("Model");
            if (modelNode == null)
            {
                throw new XmlException("Could not find Model node while loading TextComponent.");
            }

            Model model = modelLibrary.Get(modelNode.Value) ?? throw new XmlException("Invalid Model in TextComponent.");

            XElement? layerNode = componentNode.Element("Layer");
            if(layerNode == null)
            {
                logger.LogWarn("Could not find Layer node while loading TextComponent. Assuming 0.");
            }

            int layer = int.Parse(layerNode?.Value ?? "0");

            XElement? shaderNode = componentNode.Element("Shader");
            if(shaderNode == null)
            {
                throw new XmlException("Could not find Shader node while loading TextComponent.");
            }

            IShader shader = shaderLibrary.Get(shaderNode.Value);

            return new TextComponent(text, font, renderer, model, layer, shader);
        }
    }
}
