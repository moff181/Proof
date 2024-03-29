﻿using System.Xml;
using System.Xml.Serialization;

namespace Proof.Core.ProjectStructure
{
    public struct ProgramFile
    {
        public string StartupScene { get; set; }

        public void Save(string filePath)
        {
            var xsSubmit = new XmlSerializer(typeof(ProgramFile));

            using var sw = new StreamWriter(filePath);
            using var writer = XmlWriter.Create(sw);
                
            xsSubmit.Serialize(writer, this);
        }

        public static ProgramFile? Load(string filePath)
        {
            var ser = new XmlSerializer(typeof(ProgramFile));
            using XmlReader reader = XmlReader.Create(filePath);
            return (ProgramFile?)ser.Deserialize(reader);
        }

        public static ProgramFile CreateDefault()
        {
            const string defaultStartupScene = "res/scenes/Default.scene";

            return new ProgramFile
            {
                StartupScene = defaultStartupScene,
            };
        }
    }
}
