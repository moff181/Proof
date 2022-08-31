using Proof.Render;
using System.Numerics;

namespace Proof.Entities.Components
{
    public class CameraComponent : IComponent
    {
        public bool Active { get; set; }

        private readonly Shader _shader;
        
        public CameraComponent(Shader shader, bool active)
        {
            _shader = shader;
            Active = active;
        }

        public void Update(Entity entity)
        {
            if(!Active)
            {
                return;
            }

            var transformComponent = entity.GetComponent<TransformComponent>();

            Vector2 position = transformComponent?.Position ?? new Vector2(0, 0);
            Vector2 scale = transformComponent?.Scale ?? new Vector2(1, 1);

            Matrix4x4 scaleMat = Matrix4x4.CreateScale(scale.X, scale.Y, 1);
            Matrix4x4 translationMat = Matrix4x4.CreateTranslation(position.X, position.Y, 0);
            Matrix4x4 transformationMat = scaleMat * translationMat;

            _shader.LoadUniform("Transformation", transformationMat);
        }
    }
}
