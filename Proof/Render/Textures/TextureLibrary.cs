using Proof.Core.Logging;
using Proof.OpenGL;

namespace Proof.Render.Textures
{
    public class TextureLibrary
    {
        private readonly IOpenGLApi _gl;
        private readonly ALogger _logger;
        private readonly Dictionary<string, Texture> _textures;

        public TextureLibrary(IOpenGLApi gl, ALogger logger)
        {
            _gl = gl;
            _logger = logger;
            _textures = new Dictionary<string, Texture>();
        }

        public Texture Get(string path)
        {
            if(_textures.TryGetValue(path, out Texture? result))
            {
                return result;
            }

            var newResult = new Texture(_gl, _logger, path);
            _textures[path] = newResult;

            return newResult;
        }
    }
}
