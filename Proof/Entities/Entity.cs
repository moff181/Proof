using Proof.Audio;
using Proof.Core.Logging;
using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using Proof.Render.Textures;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities
{
    public class Entity
    {
        private List<AudioComponent> _audioComponent;
        private CameraComponent? _cameraComponent;
        private ColourComponent? _colourComponent;
        private RenderableComponent? _renderableComponent;
        private List<ScriptComponent> _scriptComponent;
        private TextureComponent? _textureComponent;
        private TransformComponent? _transformComponent;

        public Entity(string name)
        {
            _audioComponent = new List<AudioComponent>();
            _scriptComponent = new List<ScriptComponent>();
            Name = name;
        }

        public string Name { get; }

        public void Update()
        {
            _audioComponent.ForEach(x => x.Update(this));
            _cameraComponent?.Update(this);
            _colourComponent?.Update(this);
            _renderableComponent?.Update(this);
            _scriptComponent.ForEach(x => x.Update(this));
            _textureComponent?.Update(this);
            _transformComponent?.Update(this);
        }

        public void AddComponent(IComponent component)
        {
            if (component is AudioComponent audioComp)
            {
                _audioComponent.Add(audioComp);
            }
            else if (component is CameraComponent cameraComp)
            {
                _cameraComponent = cameraComp;
            }
            else if (component is ColourComponent colourComp)
            {
                _colourComponent = colourComp;
            }
            else if (component is RenderableComponent renderableComp)
            {
                _renderableComponent = renderableComp;
            }
            else if (component is ScriptComponent scriptComp)
            {
                _scriptComponent.Add(scriptComp);
            }
            else if (component is TextureComponent textureComp)
            {
                _textureComponent = textureComp;
            }
            else if (component is TransformComponent transformComp)
            {
                _transformComponent = transformComp;
            }
            else
            {
                throw new ArgumentException($"Unknown component type could not be added.");
            }
        }

        public void RemoveComponent(IComponent component)
        {
            if (component is AudioComponent audioComp)
            {
                _audioComponent.Remove(audioComp);
            }
            else if (component is CameraComponent)
            {
                _cameraComponent = null;
            }
            else if (component is ColourComponent)
            {
                _colourComponent = null;
            }
            else if (component is RenderableComponent)
            {
                _renderableComponent = null;
            }
            else if (component is ScriptComponent scriptComp)
            {
                _scriptComponent.Remove(scriptComp);
            }
            else if (component is TextureComponent)
            {
                _textureComponent = null;
            }
            else if (component is TransformComponent)
            {
                _transformComponent = null;
            }
            else
            {
                throw new ArgumentException($"Unknown component type could not be added.");
            }
        }

        public T? GetComponent<T>() where T : IComponent
        {
            if (typeof(T) == typeof(AudioComponent))
            {
                return (T?)Convert.ChangeType(_audioComponent.FirstOrDefault(), typeof(T?));
            }
            if (typeof(T) == typeof(CameraComponent))
            {
                return (T?)Convert.ChangeType(_cameraComponent, typeof(T?));
            }
            if (typeof(T) == typeof(ColourComponent))
            {
                return (T?)Convert.ChangeType(_colourComponent, typeof(T?));
            }
            if (typeof(T) == typeof(RenderableComponent))
            {
                return (T?)Convert.ChangeType(_renderableComponent, typeof(T?));
            }
            if (typeof(T) == typeof(ScriptComponent))
            {
                return (T?)Convert.ChangeType(_scriptComponent.FirstOrDefault(), typeof(T?));
            }
            if (typeof(T) == typeof(TextureComponent))
            {
                return (T?)Convert.ChangeType(_textureComponent, typeof(T?));
            }
            if (typeof(T) == typeof(TransformComponent))
            {
                return (T?)Convert.ChangeType(_transformComponent, typeof(T?));
            }

            throw new ArgumentException("Unknown type for retrieving component");
        }

        public AudioComponent? GetAudio(string path)
        {
            return _audioComponent.FirstOrDefault(x => x.Sound.Path == path);
        }

        public ScriptComponent? GetScript(string scriptName)
        {
            return _scriptComponent.FirstOrDefault(x => x.ScriptName == scriptName);
        }

        public IEnumerable<IComponent> GetComponents()
        {
            // TODO: update this to work with attributes or something
            var result = new List<IComponent?>
            {
                _cameraComponent,
                _colourComponent,
                _renderableComponent,
                _textureComponent,
                _transformComponent,
            };
            result.AddRange(_audioComponent);
            result.AddRange(_scriptComponent);

            return result
                .Where(x => x != null)
                .Cast<IComponent>();
        }

        public XElement ToXml()
        {
            var entity = new XElement(
                "Entity",
                new XElement("Name", Name));

            var components = new XElement("Components");

            foreach (var component in GetComponents())
            {
                components.Add(component.ToXml());
            }

            entity.Add(components);

            return entity;
        }

        public static Entity LoadFromNode(
            ALogger logger,
            Shader shader,
            ModelLibrary modelLibrary,
            Renderer renderer,
            VertexLayout layout,
            InputManager inputManager,
            ScriptLoader scriptLoader,
            SoundLibrary soundLibrary,
            TextureLibrary textureLibrary,
            XElement node)
        {
            XElement? nameNode = node.Element("Name");
            if(nameNode == null)
            {
                throw new XmlException("Entity node was missing the Name node.");
            }

            var entity = new Entity(nameNode.Value);

            XElement? componentsNode = node.Element("Components");
            if(componentsNode == null)
            {
                logger.LogWarn("Could not find Components node while loading entity. Creating with no components.");
                return entity;
            }

            var componentLoader = new ComponentLoader(logger);
            foreach (XElement componentNode in componentsNode.Elements())
            {
                entity.AddComponent(
                    componentLoader.LoadFromNode(
                        shader,
                        modelLibrary,
                        renderer,
                        layout,
                        inputManager,
                        scriptLoader,
                        soundLibrary,
                        textureLibrary,
                        componentNode));
            }

            return entity;
        }
    }
}
