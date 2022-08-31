using Proof.Core.Logging;
using Proof.Entities.Components;
using Proof.Render;
using System.Xml.Linq;

namespace Proof.Entities
{
    public class Entity
    {
        private readonly IList<IComponent> _components;

        public Entity()
        {
            _components = new List<IComponent>();
        }

        public void Update()
        {
            foreach (IComponent current in _components)
            {
                current.Update(this);
            }
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public T? GetComponent<T>()
        {
            return (T?)_components.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public static Entity LoadFromNode(ALogger logger, Shader shader, XElement node)
        {
            var entity = new Entity();

            XElement? componentsNode = node.Element("Components");
            if(componentsNode == null)
            {
                logger.LogWarn("Could not find Components node while loading entity. Creating with no components.");
                return entity;
            }

            var componentLoader = new ComponentLoader(logger);
            foreach (XElement componentNode in componentsNode.Elements("Component"))
            {
                entity.AddComponent(componentLoader.LoadFromNode(shader, componentNode));
            }

            return entity;
        }
    }
}
