using Proof.Core.Logging;
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
                    return LoadScriptComponent(componentNode, inputManager);
                default:
                    throw new NotSupportedException($"Unable to load component node with name: {componentNode}");
            }
        }

        private IComponent LoadScriptComponent(XElement componentNode, InputManager inputManager)
        {
            XElement? classNode = componentNode.Element("Class");
            if (classNode == null)
            {
                throw new XmlException("Could not find Class node in ScriptElement.");
            }

            string? className = classNode.Value;

            Assembly? assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                throw new EntryPointNotFoundException("Could not find entry assembly while loading ScriptComponent");
            }

            Type? t = assembly.GetType(className);
            if (t == null)
            {
                throw new TypeLoadException($"Could not load type specified in ScriptComponent: {className}");
            }

            if (!t.GetInterfaces().Contains(typeof(IComponent)))
            {
                throw new TypeLoadException($"Class specified in ScriptComponent was invalid as it didn't implement IComponent: {className}");
            }

            foreach(ConstructorInfo constructorInfo in t.GetConstructors())
            {
                if (!constructorInfo.IsPublic)
                {
                    continue;
                }

                ParameterInfo[] paramInfos = constructorInfo.GetParameters();
                var parameters = new List<object>();
                foreach (ParameterInfo parameter in paramInfos)
                {
                    if(parameter.ParameterType == typeof(InputManager))
                    {
                        parameters.Add(inputManager);
                    }
                }

                if(parameters.Count() != paramInfos.Length)
                {
                    continue;
                }

                return (IComponent)constructorInfo.Invoke(parameters.ToArray());
            }

            throw new TypeLoadException($"Could not find a suitable constructor for type specified in ScriptComponent: {className}");
        }
    }
}
