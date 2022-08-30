using Proof.Render;
using Proof.Render.Buffers;

namespace Proof.Entities.Components
{
    public class RenderableComponent : IComponent
    {
        private readonly Renderer _renderer;
        private readonly VertexLayout _layout;
        private readonly Model _model;

        public RenderableComponent(Renderer renderer, VertexLayout layout, Model model)
        {
            _renderer = renderer;
            _layout = layout;
            _model = model;
        }

        public void Update(Entity entity)
        {            
            var vertices = new float[_model.Vertices.Length];
            Array.Copy(_model.Vertices, vertices, vertices.Length);

            var transformComponent = entity.GetComponent<TransformComponent>();
            if (transformComponent != null)
            {
                for (int i = _layout.PositionIndex; i < vertices.Length; i += _layout.SumOfElements())
                {
                    vertices[i] = transformComponent.Scale.X * vertices[i] + transformComponent.Position.X;
                    vertices[i+1] = transformComponent.Scale.Y * vertices[i+1] + transformComponent.Position.Y;
                }
            }

            _renderer.Submit(vertices, _model.Indices);
        }
    }
}
