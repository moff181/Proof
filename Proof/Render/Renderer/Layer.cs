using Proof.Render.Buffers;
using Proof.Render.Shaders;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public class Layer
    {
        private readonly Dictionary<IShader, Dictionary<ITexture, List<RenderData>>> _renderables;

        public Layer()
        {
            _renderables = new Dictionary<IShader, Dictionary<ITexture, List<RenderData>>>();
        }

        public void Add(RenderData renderData, ITexture texture, IShader shader)
        {
            if (_renderables.TryGetValue(shader, out Dictionary<ITexture, List<RenderData>>? textureGroup))
            {
                if (textureGroup.TryGetValue(texture, out List<RenderData>? shaderExistsList))
                {
                    shaderExistsList.Add(renderData);
                }
                else
                {
                    var newList = new List<RenderData>();
                    newList.Add(renderData);
                    textureGroup.Add(texture, newList);
                }

                return;
            }

            var newTextureGroup = new Dictionary<ITexture, List<RenderData>>();
            var newTextureGroupNewList = new List<RenderData>();
            newTextureGroupNewList.Add(renderData);
            newTextureGroup.Add(texture, newTextureGroupNewList);
            _renderables.Add(shader, newTextureGroup);
        }

        public void Render(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, ref int textureSlot)
        {
            foreach (IShader shader in _renderables.Keys)
            {
                shader.Bind();
                shader.PrepareTextureUniform();
                foreach (ITexture texture in _renderables[shader].Keys)
                {
                    bool isNoTexture = texture == NoTexture.Instance;
                    if (!isNoTexture)
                    {
                        texture.Bind(textureSlot);
                    }

                    int localTextureSlot = isNoTexture ? -1 : textureSlot;
                    foreach (RenderData renderData in _renderables[shader][texture])
                    {
                        UpdateTexSlot(shader.GetLayout(), renderData.Vertices, localTextureSlot);
                        vertexBuffer.Submit(renderData.Vertices);
                        indexBuffer.Submit(renderData.Indices);
                    }
                    textureSlot++;
                }

                vertexBuffer.Flush(shader.GetLayout());
                indexBuffer.Flush();
            }
        }

        private static void UpdateTexSlot(VertexLayout layout, float[] vertices, int texSlotValue)
        {
            int? texSlot = layout.TextureSlotIndex;
            if(texSlot == null)
            {
                return;
            }

            int sumOfElements = layout.SumOfElements();
            for (int i = 0; i < vertices.Length; i += sumOfElements)
            {
                vertices[i + texSlot.Value] = texSlotValue;
            }
        }
    }
}
