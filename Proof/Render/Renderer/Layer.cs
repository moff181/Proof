using Proof.Render.Buffers;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public class Layer
    {
        private readonly Dictionary<Texture, List<RenderData>> _renderables;

        public Layer()
        {
            _renderables = new Dictionary<Texture, List<RenderData>>();
        }

        public void Add(RenderData renderData, Texture texture)
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

        public void Render(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            foreach(Texture texture in _renderables.Keys)
            {
                // TODO: update the bind index here
                texture.Bind(0);

                foreach(RenderData renderData in _renderables[texture])
                {
                    vertexBuffer.Submit(renderData.Vertices);
                    indexBuffer.Submit(renderData.Indices);
                }
            }
        }
    }
}
