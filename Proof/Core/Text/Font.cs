using Proof.Render.Textures;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Proof.Core.Text
{
    public class Font
    {
        private readonly Dictionary<int, FontCharacter> _characters;

        private Font(string name, int lineHeight, Texture texture, Dictionary<int, FontCharacter> characters, int textureWidth, int textureHeight, int baseVal)
        {
            Name = name;
            LineHeight = lineHeight;
            Texture = texture;
            _characters = characters;
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            Base = baseVal;
        }

        public string Name { get; }
        public int LineHeight { get; }
        public Texture Texture { get; }
        public int TextureWidth { get; }
        public int TextureHeight { get; }
        public int Base { get; }

        public FontCharacter GetCharacterInformation(char c)
        {
            return _characters[c];
        }

        public static Font LoadFromFile(string filePath, TextureLibrary textureLibrary)
        {
            string[] lines = File.ReadAllLines(filePath);

            string name = string.Empty;
            int lineHeight = 0;
            string textureFile = string.Empty;
            var characters = new Dictionary<int, FontCharacter>();
            int textureWidth = 0;
            int textureHeight = 0;
            int baseVal = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if(i == 0)
                {
                    name = GetSpaceIndexedInformation(line, "face").Replace("\"", "");
                }
                else if(i == 1)
                {
                    lineHeight = int.Parse(GetSpaceIndexedInformation(line, "lineHeight"));
                    textureWidth = int.Parse(GetSpaceIndexedInformation(line, "scaleW"));
                    textureHeight = int.Parse(GetSpaceIndexedInformation(line, "scaleH"));
                    baseVal = int.Parse(GetSpaceIndexedInformation(line, "base"));
                }
                else if(i == 2)
                {
                    textureFile = GetSpaceIndexedInformation(line, "file").Replace("\"", ""); ;
                }
                else if(i != 3)
                {
                    FontCharacter character = ParseCharacter(line);
                    characters.Add(character.Id, character);
                }
            }

            return new Font(
                name,
                lineHeight,
                GetTexture(filePath, textureFile, textureLibrary),
                characters,
                textureWidth,
                textureHeight,
                baseVal);
        }

        private static FontCharacter ParseCharacter(string line)
        {
            string singleWhiteSpace= Regex.Replace(line, @"\s+", " ");

            int getInfo(string info) => int.Parse(GetSpaceIndexedInformation(singleWhiteSpace, info));

            return new FontCharacter(
                getInfo("id"),
                getInfo("x"),
                getInfo("y"),
                getInfo("width"),
                getInfo("height"),
                getInfo("xoffset"),
                getInfo("yoffset"),
                getInfo("xadvance"));
        }

        private static string GetSpaceIndexedInformation(string line, string infoId)
        {
            return line.Split(' ')
                .Where(x => x.Contains('='))
                .First(x => x.Split('=')[0] == infoId)
                .Split('=')[1];
        }

        private static Texture GetTexture(string fntFilePath, string texturePath, TextureLibrary textureLibrary)
        {
            string directoryPath = fntFilePath.Substring(0, fntFilePath.LastIndexOf('/'));
            string fullPath = Path.Combine(directoryPath, texturePath);
            return textureLibrary.Get(fullPath);
        }
    }
}
