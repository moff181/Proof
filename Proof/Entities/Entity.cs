using Proof.Entities.Components;
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

        public static Entity LoadFromNode(XElement node)
        {
            var entity = new Entity();

            return entity;
        }
    }
}
