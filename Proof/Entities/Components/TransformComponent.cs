using System.Numerics;

namespace Proof.Entities.Components
{
    public class TransformComponent : IComponent
    {
        public TransformComponent(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }

        public TransformComponent()
            : this(new Vector2(0, 0), new Vector2(1, 1))
        { }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public void Update(Entity entity)
        { }
    }
}
