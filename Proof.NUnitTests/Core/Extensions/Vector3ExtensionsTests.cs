using NUnit.Framework;
using Proof.Core.Extensions;
using System.Numerics;
using System.Xml.Linq;

namespace Proof.NUnitTests.Core.Extensions
{
    [TestFixture]
    public class Vector3ExtensionsTests
    {
        private static readonly XElement[] Expected = new XElement[]
        {
            new XElement("NodeName", new XElement("X", 0), new XElement("Y", 0), new XElement("Z", 0)),
            new XElement("NodeName", new XElement("X", 1), new XElement("Y", -1), new XElement("Z", 0)),
            new XElement("Node", new XElement("X", -1), new XElement("Y", 1), new XElement("Z", -2)),
            new XElement("Node", new XElement("X", -1.5f), new XElement("Y", 1.5f), new XElement("Z", 2.5f)),
            new XElement("Name", new XElement("X", 1.5f), new XElement("Y", -1.5f), new XElement("Z", -2.5f)),
            new XElement("Name", new XElement("X", 1.5f), new XElement("Y", -2.5f), new XElement("Z", 3.5f)),
            new XElement("MyNode", new XElement("X", 2.5f), new XElement("Y", -1.5f), new XElement("Z", 5)),
            new XElement("MyNode", new XElement("X", 1.25f), new XElement("Y", 0), new XElement("Z", -2.5f)),
        };

        [TestCase(0, 0, 0, "NodeName", 0)]
        [TestCase(1, -1, 0, "NodeName", 1)]
        [TestCase(-1, 1, -2, "Node", 2)]
        [TestCase(-1.5f, 1.5f, 2.5f, "Node", 3)]
        [TestCase(1.5f, -1.5f, -2.5f, "Name", 4)]
        [TestCase(1.5f, -2.5f, 3.5f, "Name", 5)]
        [TestCase(2.5f, -1.5f, 5, "MyNode", 6)]
        [TestCase(1.25f, 0, -2.5f, "MyNode", 7)]
        public void ShouldConvertToXmlCorrectly(float x, float y, float z, string nodeName, int expectedId)
        {
            var vector3 = new Vector3(x, y, z);

            string expected = Expected[expectedId].ToString();
            Assert.AreEqual(expected, vector3.ToXml(nodeName).ToString());
        }
    }
}
