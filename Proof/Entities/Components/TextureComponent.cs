using Proof.Render.Textures;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class TextureComponent : IComponent
    {
        public TextureComponent(Texture texture)
        {
            Texture = texture;
        }

        public Texture Texture { get; set; }

        public void Update(Entity entity)
        {
            // Do nothing
        }

        public XElement ToXml()
        {
            return new XElement(
                "TextureComponent",
                new XElement("FilePath", Texture.FilePath));
        }

        public static TextureComponent LoadFromXml(TextureLibrary textureLibrary, XElement componentNode)
        {
            XElement? filePathNode = componentNode.Element("FilePath");
            if (filePathNode == null)
            {
                throw new XmlException("Could not find FilePath node for TextureComponent.");
            }

            string filePath = filePathNode.Value;
            Texture texture = textureLibrary.Get(filePath);

            return new TextureComponent(texture);
        }
    }
}
