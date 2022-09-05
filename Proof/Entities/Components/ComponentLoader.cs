using Proof.Core.Logging;
using Proof.Entities.Components.ScriptLoaders;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using System.Numerics;
using System.Reflection;
using System.Xml;
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
            IScriptLoader scriptLoader,
            XElement componentNode)
        {
            string name = componentNode.Name.LocalName;

            switch(name)
            {
                case "CameraComponent":
                    return CameraComponent.LoadFromNode(_logger, shader, componentNode);
                case "RenderableComponent":
                    return RenderableComponent.LoadFromNode(_logger, modelLibrary, renderer, layout, componentNode);
                case "TransformComponent":
                    return TransformComponent.LoadFromNode(_logger, componentNode);
                case "ScriptComponent":
                    return scriptLoader.LoadScriptComponent(componentNode, inputManager);
                default:
                    throw new NotSupportedException($"Unable to load component node with name: {componentNode}");
            }
        }
    }
}
