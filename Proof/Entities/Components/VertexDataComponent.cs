using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class VertexDataComponent : IComponent
    {
        public VertexDataComponent(Dictionary<int, float> data)
        {
            Data = data;
        }

        // index -> value
        public Dictionary<int, float> Data { get; }

        public void Update(Entity entity)
        {
            // Do nothing
        }

        public XElement ToXml()
        {
            var element = new XElement("VertexDataComponent");

            foreach(KeyValuePair<int, float> pair in Data)
            {
                element.Add(
                    new XElement(
                        "VertexData",
                        new XElement("Index", pair.Key),
                        new XElement("InitialValue", pair.Value)));
            }

            return element;
        }

        public static IComponent LoadFromNode(XElement componentNode)
        {
            var dict = new Dictionary<int, float>();

            foreach(XElement vertexDataNode in componentNode.Elements("VertexData"))
            {
                XElement? indexNode = vertexDataNode.Element("Index");
                if(indexNode == null)
                {
                    throw new XmlException("Could not find Index node while loading VertexDataComponent.");
                }

                XElement? initialValueNode = vertexDataNode.Element("InitialValue");
                if(initialValueNode == null)
                {
                    throw new XmlException("Could not find Index node while loading VertexDataComponent.");
                }

                if (!int.TryParse(indexNode.Value, out int index))
                {
                    throw new XmlException("Value of Index node was invalid in VertexDataComponent.");
                }

                if (!float.TryParse(initialValueNode.Value, out float initialValue))
                {
                    throw new XmlException("Value of InitialValue node was invalid in VertexDataComponent.");
                }

                dict.Add(index, initialValue);
            }

            return new VertexDataComponent(dict);
        }
    }
}
