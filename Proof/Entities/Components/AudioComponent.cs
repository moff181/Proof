using Proof.Audio;
using Proof.Core.Logging;
using Proof.Render.Shaders;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class AudioComponent : IComponent
    {
        public AudioComponent(Sound sound)
        {
            Sound = sound;
        }

        public Sound Sound { get; }

        public void Update(Entity entity)
        {
            // Do nothing
        }

        public XElement ToXml()
        {
            return new XElement(
                "AudioComponent",
                new XElement(
                    "FilePath",
                    Sound.Path));
        }

        public static IComponent LoadFromNode(SoundLibrary soundLibrary, XElement componentNode)
        {
            XElement? filePathNode = componentNode.Element("FilePath");
            if (filePathNode == null)
            {
                throw new XmlException("Could not find FilePath node for AudioComponent.");
            }

            string filePath = filePathNode.Value;
            Sound sound = soundLibrary.Get(filePath);

            return new AudioComponent(sound);
        }
    }
}
