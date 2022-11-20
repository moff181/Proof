using Proof.Core.Logging;
using Proof.Render.Shaders;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class CameraComponent : IComponent
    {
        public bool Active { get; set; }

        public IShader Shader { get; set; }
        
        public CameraComponent(IShader shader, bool active)
        {
            Shader = shader;
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

            Shader.Bind();
            Shader.LoadUniform("Transformation", transformationMat);
        }

        public XElement ToXml()
        {
            return new XElement(
                "CameraComponent",
                new XElement("Active", Active),
                new XElement("Shader", Shader.FilePath));
        }

        public static IComponent LoadFromNode(ALogger logger, IShader[] shaders, XElement componentNode)
        {
            XElement? activeNode = componentNode.Element("Active");
            if(activeNode == null)
            {
                logger.LogWarn("Could not fine Active node while loading CameraComponent. Assuming false.");
            }

            bool active = bool.Parse(activeNode?.Value ?? "false");

            XElement? shaderNode = componentNode.Element("Shader");
            if(shaderNode == null)
            {
                throw new XmlException("Could not find shader node while parsing CameraComponent.");
            }

            IShader? shader = shaders.FirstOrDefault(x => x.FilePath == shaderNode.Value);
            if(shader == null)
            {
                throw new IOException($"Could not find Shader specified in CameraComponent: {shaderNode.Value}");
            }

            return new CameraComponent(shader, active);
        }
    }
}
