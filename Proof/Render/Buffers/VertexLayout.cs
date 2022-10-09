using Proof.Core.Logging;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Render.Buffers
{
    public class VertexLayout : IEnumerable<int>
    {
        private readonly IList<int> _arraySizes;
        private int _sumOfElements;

        public VertexLayout(int positionIndex, int? colourIndex)
        {
            _arraySizes = new List<int>();
            PositionIndex = positionIndex;
            ColourIndex = colourIndex;
        }

        public int PositionIndex { get; }
        public int? ColourIndex { get; }

        public void AddArray(int arraySize)
        {
            _arraySizes.Add(arraySize);
            _sumOfElements = _arraySizes.Sum();
        }

        public int Count()
        {
            return _arraySizes.Count;
        }

        public int SumOfElements()
        {
            return _sumOfElements;
        }

        public int Stride()
        {
            return SumOfElements() * sizeof(float);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _arraySizes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static VertexLayout LoadFromNode(ALogger logger, XElement vertexLayoutNode)
        {
            XElement? positionIndexNode = vertexLayoutNode.Element("PositionIndex");
            if (positionIndexNode == null)
            {
                throw new XmlException("Could not find PositionIndex node in vertex layout file.");
            }

            if (!int.TryParse(positionIndexNode.Value, out int positionIndex))
            {
                throw new XmlException($"Value stored in PositionIndex node was invalid: {positionIndexNode.Value}");
            }

            int? colourIndex = null;
            XElement? colourIndexNode = vertexLayoutNode.Element("ColourIndex");
            if (colourIndexNode != null && int.TryParse(colourIndexNode.Value, out int colourIndexTemp))
            {
                colourIndex = colourIndexTemp;
            }

            var vertexLayout = new VertexLayout(positionIndex, colourIndex);

            XElement? attribArraysNode = vertexLayoutNode.Element("AttribArrays");
            if(attribArraysNode == null)
            {
                logger.LogWarn("AttribArrays node was missing in vertex layout file. Skipping adding attrib arrays.");
                return vertexLayout;
            }

            var attribArrayCollection = attribArraysNode.Elements("AttribArray");
            foreach(var attribArray in attribArrayCollection)
            {
                if(!int.TryParse(attribArray.Value, out int val))
                {
                    logger.LogWarn($"AttribArray had invalid value in vertex layout file ({attribArray.Value}). Ignoring.");
                    continue;
                }

                vertexLayout.AddArray(val);
            }

            logger.LogInfo("Vertex layout loaded.");
            return vertexLayout;
        }
    }
}
