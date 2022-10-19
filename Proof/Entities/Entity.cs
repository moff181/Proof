using Proof.Audio;
using Proof.Core.Logging;
using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities
{
    public class Entity
    {
        private readonly Dictionary<Type, IComponent> _components;

        public Entity(string name)
        {
            _components = new Dictionary<Type, IComponent>();
            Name = name;
        }

        public string Name { get; }

        public void Update()
        {
            foreach (IComponent current in _components.Values)
            {
                current.Update(this);
            }
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component.GetType(), component);
        }

        public void RemoveComponent(IComponent component)
        {
            _components.Remove(component.GetType());
        }

        public T? GetComponent<T>()
        {
            if(_components.TryGetValue(typeof(T), out IComponent? component))
            {
                return (T)component;
            }

            return default(T);
        }

        public IEnumerable<IComponent> GetComponents()
        {
            return _components.Values;
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
                        componentNode));
            }

            return entity;
        }
    }
}
