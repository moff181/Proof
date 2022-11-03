using Proof.Core.Logging;

namespace Proof.Render.Textures
{
    public class TextureLibrary
    {
        private readonly ALogger _logger;
        private readonly Dictionary<string, Texture> _textures;

        public TextureLibrary(ALogger logger)
        {
            _logger = logger;
            _textures = new Dictionary<string, Texture>();
        }

        public Texture Get(string path)
        {
            if(_textures.TryGetValue(path, out Texture? result))
            {
                return result;
            }

            return new Texture(_logger, path);
        }
    }
}
