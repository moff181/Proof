using Proof.Audio;
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
                    Sound.Path);
        }
    }
}
