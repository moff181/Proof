using Proof.Core.Extensions;
using Proof.Core.Logging;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class ColourComponent : IComponent
    {
        public ColourComponent(Vector3 colour)
        {
            Colour = colour;
        }

        public Vector3 Colour { get; set; }

        public void Update(Entity entity)
        {
            // Do nothing.
        }

        public XElement ToXml()
        {
            return Colour.ToXml("ColourComponent");
        }

        public static IComponent LoadFromNode(XElement componentNode)
        {
            var xNode = componentNode.Element("X");
            if(xNode == null || !float.TryParse(xNode.Value, out float x))
            {
                throw new XmlException("Could not load X node from ColourComponent.");
            }

            var yNode = componentNode.Element("Y");
            if (yNode == null || !float.TryParse(yNode.Value, out float y))
            {
                throw new XmlException("Could not load Y node from ColourComponent.");
            }

            var zNode = componentNode.Element("Z");
            if (zNode == null || !float.TryParse(zNode.Value, out float z))
            {
                throw new XmlException("Could not load Z node from ColourComponent.");
            }

            return new ColourComponent(new Vector3(x, y, z));
        }
    }
}
