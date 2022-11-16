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
        private readonly List<AudioComponent> _audioComponent;
        private readonly List<ScriptComponent> _scriptComponent;
        private readonly Dictionary<Type, IComponent> _otherComponents;

        public Entity(string name)
        {
            _audioComponent = new List<AudioComponent>();
            _scriptComponent = new List<ScriptComponent>();
            _otherComponents = new Dictionary<Type, IComponent>();
            Name = name;
        }

        public string Name { get; }

        public void Update()
        {
            _audioComponent.ForEach(x => x.Update(this));
            _scriptComponent.ForEach(x => x.Update(this));
            
            foreach(var comp in _otherComponents.Values)
            {
                comp.Update(this);
            }
        }

        public void AddComponent(IComponent component)
        {
            if (component is AudioComponent audioComp)
            {
                _audioComponent.Add(audioComp);
            }
            else if (component is ScriptComponent scriptComp)
            {
                _scriptComponent.Add(scriptComp);
            }
            else
            {
                _otherComponents.Add(component.GetType(), component);
            }
        }

        public void RemoveComponent(IComponent component)
        {
            if (component is AudioComponent audioComp)
            {
                _audioComponent.Remove(audioComp);
            }
            else if (component is ScriptComponent scriptComp)
            {
                _scriptComponent.Remove(scriptComp);
            }
            else
            {
                _otherComponents.Remove(component.GetType());
            }
        }

        public T? GetComponent<T>() where T : IComponent
        {
            if (typeof(T) == typeof(AudioComponent))
            {
                return (T?)Convert.ChangeType(_audioComponent.FirstOrDefault(), typeof(T?));
            }
           
            if (typeof(T) == typeof(ScriptComponent))
            {
                return (T?)Convert.ChangeType(_scriptComponent.FirstOrDefault(), typeof(T?));
            }

            if (_otherComponents.TryGetValue(typeof(T), out IComponent? component))
            {
                return (T)component;
            }

            return default(T);
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
            var result = new List<IComponent?>();
            result.AddRange(_audioComponent);
            result.AddRange(_scriptComponent);
            result.AddRange(_otherComponents.Values);

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
            IShader[] shaders,
            ModelLibrary modelLibrary,
            Renderer renderer,
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
                        shaders,
                        modelLibrary,
                        renderer,
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
