using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class ComponentLoader
    {
        private readonly ALogger _logger;

        public ComponentLoader(ALogger logger)
        {
            _logger = logger;
        }

        public IComponent LoadFromNode(
            Shader shader,
            ModelLibrary modelLibrary,
            Renderer renderer,
            VertexLayout layout,
            InputManager inputManager,
            ScriptLoader scriptLoader,
            XElement componentNode)
        {
            string name = componentNode.Name.LocalName;

            switch(name)
            {
                case "CameraComponent":
                    return CameraComponent.LoadFromNode(_logger, shader, componentNode);
                case "ColourComponent":
                    return ColourComponent.LoadFromNode(componentNode);
                case "RenderableComponent":
                    return RenderableComponent.LoadFromNode(_logger, modelLibrary, renderer, layout, componentNode);
                case "ScriptComponent":
                    return ScriptComponent.LoadFromXml(componentNode, scriptLoader, inputManager);
                case "TransformComponent":
                    return TransformComponent.LoadFromNode(_logger, componentNode);
                default:
                    throw new NotSupportedException($"Unable to load component node with name: {componentNode}");
            }
        }
    }
}
