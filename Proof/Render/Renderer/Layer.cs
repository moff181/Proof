using Proof.Render.Buffers;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public class Layer
    {
        private readonly Dictionary<ITexture, List<RenderData>> _renderables;

        public Layer()
        {
            _renderables = new Dictionary<ITexture, List<RenderData>>();
        }

        public void Add(RenderData renderData, ITexture texture)
        {
            if(_renderables.TryGetValue(texture, out List<RenderData>? list))
            {
                list.Add(renderData);
            } 
            else
            {
                var newList = new List<RenderData>();
                newList.Add(renderData);
                _renderables.Add(texture, newList);
            }
        }

        public void Render(VertexLayout layout, VertexBuffer vertexBuffer, IndexBuffer indexBuffer, ref int textureSlot)
        {
            foreach(ITexture texture in _renderables.Keys)
            {
                bool isNoTexture = texture == NoTexture.Instance;
                if(!isNoTexture)
                {
                    texture.Bind(textureSlot);
                }

                int localTextureSlot = isNoTexture ? -1 : textureSlot;
                foreach(RenderData renderData in _renderables[texture])
                {
                    UpdateTexSlot(layout, renderData.Vertices, localTextureSlot);
                    vertexBuffer.Submit(renderData.Vertices);
                    indexBuffer.Submit(renderData.Indices);
                }
                textureSlot++;
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
