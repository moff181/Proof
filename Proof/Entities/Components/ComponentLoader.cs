using Proof.Audio;
using Proof.Core.Logging;
using Proof.Core.Text;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using Proof.Render.Textures;
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
            ShaderLibrary shaderLibrary,
            ModelLibrary modelLibrary,
            Renderer renderer,
            InputManager inputManager,
            ScriptLoader scriptLoader,
            SoundLibrary soundLibrary,
            TextureLibrary textureLibrary,
            FontLibrary fontLibrary,
            XElement componentNode)
        {
            string name = componentNode.Name.LocalName;

            switch(name)
            {
                case "AudioComponent":
                    return AudioComponent.LoadFromNode(soundLibrary, componentNode);
                case "CameraComponent":
                    return CameraComponent.LoadFromNode(_logger, shaderLibrary, componentNode);
                case "ColourComponent":
                    return ColourComponent.LoadFromNode(componentNode);
                case "RenderableComponent":
                    return RenderableComponent.LoadFromNode(_logger, modelLibrary, renderer, shaderLibrary, componentNode);
                case "ScriptComponent":
                    return ScriptComponent.LoadFromXml(componentNode, scriptLoader, inputManager);
                case "TextComponent":
                    return TextComponent.LoadFromNode(_logger, fontLibrary, modelLibrary, shaderLibrary, renderer, componentNode);
                case "TextureComponent":
                    return TextureComponent.LoadFromXml(textureLibrary, componentNode);
                case "TransformComponent":
                    return TransformComponent.LoadFromNode(_logger, componentNode);
                default:
                    throw new NotSupportedException($"Unable to load component node with name: {componentNode.Name}");
            }
        }
    }
}
