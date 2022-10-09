using System.Numerics;
using System.Xml.Linq;

namespace Proof.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static XElement ToXml(this Vector3 vec, string elementName)
        {
            return new XElement(
                elementName,
                new XElement("X", vec.X),
                new XElement("Y", vec.Y),
                new XElement("Z", vec.Z));
        }
    }
}
