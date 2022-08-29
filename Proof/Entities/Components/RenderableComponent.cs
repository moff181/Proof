using Proof.Render;

namespace Proof.Entities.Components
{
    internal class RenderableComponent : IComponent
    {
        private readonly Renderer _renderer;
        private readonly Model _model;

        public RenderableComponent(Renderer renderer, Model model)
        {
            _renderer = renderer;
            _model = model;
        }

        public void Update(Entity entity)
        {
            _renderer.Submit(_model.Vertices, _model.Indices);
        }
    }
}
