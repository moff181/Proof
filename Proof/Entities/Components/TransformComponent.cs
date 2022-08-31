using Proof.Core.Logging;
using System.Numerics;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class TransformComponent : IComponent
    {
        public TransformComponent(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }

        public TransformComponent()
            : this(new Vector2(0, 0), new Vector2(1, 1))
        { }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public void Update(Entity entity)
        { }

        public static IComponent LoadFromNode(ALogger logger, XElement componentNode)
        {
            XElement? positionNode = componentNode.Element("Position");

            Vector2 position;
            if(positionNode != null)
            {
                position = ParseVector2(positionNode);
            } 
            else
            {
                logger.LogWarn("Could not find Position node while loading TransformComponent. Assuming (0, 0).");
                position = new Vector2(0, 0);
            }

            XElement? scaleNode = componentNode.Element("Scale");

            Vector2 scale;
            if (scaleNode != null)
            {
                scale = ParseVector2(scaleNode);
            }
            else
            {
                logger.LogWarn("Could not find Scale node while loading TransformComponent. Assuming (1, 1).");
                scale = new Vector2(1, 1);
            }

            return new TransformComponent(position, scale);
        }

        private static Vector2 ParseVector2(XElement node)
        {
            XElement? xNode = node.Element("X");
            XElement? yNode = node.Element("Y");

            if(xNode == null || yNode == null)
            {
                throw new ArgumentException("Could not find X and/or Y node while parsing Vector2.");
            }

            float x = float.Parse(xNode.Value);
            float y = float.Parse(yNode.Value);

            return new Vector2(x, y);
        }
    }
}
