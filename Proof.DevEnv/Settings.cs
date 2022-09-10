using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;

namespace Proof.DevEnv
{
    public struct WindowSettings
    {
        public int LeftPanelWidth { get; init; }
        public int RightPanelWidth { get; init; }
        public bool Fullscreen { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }

        public void Save(string filePath)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(WindowSettings));
            
            using (var sw = new StreamWriter(filePath))
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    xsSubmit.Serialize(writer, this);
                }
            }
        }

        public static WindowSettings? Load(string filePath)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(WindowSettings));
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    return (WindowSettings?)ser.Deserialize(reader);
                }
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
