using Proof.Core.Logging;
using Proof.Render;
using System.Numerics;
using System.Xml.Linq;

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

        public XElement ToXml()
        {
            return new XElement(
                "CameraComponent",
                new XElement("Active", Active));
        }

        public static IComponent LoadFromNode(ALogger logger, Shader shader, XElement componentNode)
        {
            XElement? activeNode = componentNode.Element("Active");
            if(activeNode == null)
            {
                logger.LogWarn("Could not fine Active node while loading CameraComponent. Assuming false.");
            }

            bool active = bool.Parse(activeNode?.Value ?? "false");
            return new CameraComponent(shader, active);
        }
    }
}
