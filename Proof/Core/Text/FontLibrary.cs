using Proof.Render.Textures;

namespace Proof.Core.Text
{
    public class FontLibrary
    {
        private readonly Dictionary<string, Font> _fonts;
        private readonly TextureLibrary _textureLibrary;

        public FontLibrary(TextureLibrary textureLibrary)
        {
            _fonts = new Dictionary<string, Font>();
            _textureLibrary = textureLibrary;
        }

        public Font Get(string filePath)
        {
            if(_fonts.TryGetValue(filePath, out Font? font))
            {
                return font;
            }

            var newFont = Font.LoadFromFile(filePath, _textureLibrary);
            _fonts.Add(filePath, newFont);

            return newFont;
        }
    }
}
