using Proof.Core.Logging;
using Proof.Render;
using Proof.Render.Buffers;
using Proof.Render.Renderer;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class RenderableComponent : IComponent
    {
        private readonly IRenderer _renderer;
        private readonly VertexLayout _layout;

        private float[] _verticesBuffer;
        private Vector2 _previousPosition;
        private Vector2 _previousScale;
        private Vector3 _previousColour;

        public RenderableComponent(IRenderer renderer, VertexLayout layout, Model model, int layer)
        {
            _renderer = renderer;
            _layout = layout;

            _previousPosition = Vector2.Zero;
            _previousScale = Vector2.One;
            _previousColour = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            Layer = layer;

            _verticesBuffer = Array.Empty<float>();
            Model = model;
            SetModel(model);
        }

        public Model Model { get; private set; }
        public int Layer { get; set; }

        public void SetModel(Model model)
        {
            Model = model;

            _verticesBuffer = new float[model.Vertices.Length];
            Array.Copy(Model.Vertices, _verticesBuffer, _verticesBuffer.Length);

            // Update _previousPosition to make it recalculate
            _previousPosition = new Vector2(float.MaxValue, float.MaxValue);
        }

        public void Update(Entity entity)
        {
            TransformComponent? transformComponent = entity.GetComponent<TransformComponent>();
            ColourComponent? colourComponent = entity.GetComponent<ColourComponent>();

            bool transformUpdate = TransformChanged(transformComponent);
            bool colourUpdate = ColourChanged(colourComponent);

            if (transformUpdate || colourUpdate)
            {
                Array.Copy(Model.Vertices, _verticesBuffer, _verticesBuffer.Length);
            }

            if (transformComponent != null && transformUpdate)
            {
                for (int i = _layout.PositionIndex; i < _verticesBuffer.Length; i += _layout.SumOfElements())
                {
                    _verticesBuffer[i] = transformComponent.Scale.X * _verticesBuffer[i] + transformComponent.Position.X;
                    _verticesBuffer[i+1] = transformComponent.Scale.Y * _verticesBuffer[i+1] + transformComponent.Position.Y;
                }

                _previousPosition = transformComponent.Position;
                _previousScale = transformComponent.Scale;
            }

            if(_layout.ColourIndex != null && colourComponent != null && colourUpdate)
            {
                for (int i = _layout.ColourIndex.Value; i < _verticesBuffer.Length; i += _layout.SumOfElements())
                {
                    _verticesBuffer[i] = colourComponent.Colour.X;
                    _verticesBuffer[i + 1] = colourComponent.Colour.Y;
                    _verticesBuffer[i + 2] = colourComponent.Colour.Z;
                }

                _previousColour = colourComponent.Colour;
            }

            _renderer.Submit(_verticesBuffer, Model.Indices, Layer);
        }

        public XElement ToXml()
        {
            return new XElement(
                "RenderableComponent",
                new XElement("Model", Model.Path),
                new XElement("Layer", Layer));
        }

        private bool TransformChanged(TransformComponent? transformComponent)
        {
            return transformComponent != null && (transformComponent.Position != _previousPosition || transformComponent.Scale != _previousScale);
        }

        private bool ColourChanged(ColourComponent? colourComponent)
        {
            return colourComponent != null && colourComponent.Colour != _previousColour;
        }

        public static IComponent LoadFromNode(
            ALogger logger,
            ModelLibrary modelLibrary,
            Renderer renderer,
            VertexLayout layout,
            XElement componentNode)
        {
            XElement? modelNode = componentNode.Element("Model");
            if (modelNode == null)
            {
                throw new XmlException("Could not find Model node while loading RenderableComponent.");
            }

            Model? model = modelLibrary.Get(modelNode.Value);
            if (model == null)
            {
                throw new IOException($"Could not load model from RenderableComponent: {modelNode.Value}");
            }

            XElement? layerNode = componentNode.Element("Layer");
            if(layerNode == null)
            {
                logger.LogWarn("Could not find Layer node while loading RenderableComponent. Assuming layer = 0.");
            }

            int layer = int.Parse(layerNode?.Value ?? "0");

            return new RenderableComponent(renderer, layout, model, layer);
        }
    }
}
