using NUnit.Framework;
using Proof.Core.Extensions;
using System.Numerics;
using System.Xml.Linq;

namespace Proof.NUnitTests.Core.Extensions
{
    [TestFixture]
    public class Vector2ExtensionsTests
    {
        private static readonly XElement[] Expected = new XElement[]
        {
            new XElement("NodeName", new XElement("X", 0), new XElement("Y", 0)),
            new XElement("NodeName", new XElement("X", 1), new XElement("Y", -1)),
            new XElement("Node", new XElement("X", -1), new XElement("Y", 1)),
            new XElement("Node", new XElement("X", -1.5f), new XElement("Y", 1.5f)),
            new XElement("Name", new XElement("X", 1.5f), new XElement("Y", -1.5f)),
            new XElement("Name", new XElement("X", 1.5f), new XElement("Y", -2.5f)),
            new XElement("MyNode", new XElement("X", 2.5f), new XElement("Y", -1.5f)),
            new XElement("MyNode", new XElement("X", 1.25f), new XElement("Y", 0)),
        };

        [TestCase(0, 0, "NodeName", 0)]
        [TestCase(1, -1, "NodeName", 1)]
        [TestCase(-1, 1, "Node", 2)]
        [TestCase(-1.5f, 1.5f, "Node", 3)]
        [TestCase(1.5f, -1.5f, "Name", 4)]
        [TestCase(1.5f, -2.5f, "Name", 5)]
        [TestCase(2.5f, -1.5f, "MyNode", 6)]
        [TestCase(1.25f, 0, "MyNode", 7)]
        public void ShouldConvertToXmlCorrectly(float x, float y, string nodeName, int expectedId)
        {
            var vector2 = new Vector2(x, y);

            string expected = Expected[expectedId].ToString();
            Assert.AreEqual(expected, vector2.ToXml(nodeName).ToString());
        }
    }
}
