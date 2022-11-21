using Proof.Core.Logging;
using Proof.OpenGL;

namespace Proof.Render.Shaders
{
    public class ShaderLibrary
    {
        private readonly IOpenGLApi _gl;
        private readonly ALogger _logger;
        private readonly Dictionary<string, Shader> _shaders;

        public ShaderLibrary(IOpenGLApi gl, ALogger logger)
        {
            _gl = gl;
            _logger = logger;
            _shaders = new Dictionary<string, Shader>();
        }

        public Shader Get(string path)
        {
            if (_shaders.TryGetValue(path, out Shader? result))
            {
                return result;
            }

            var newResult = Shader.LoadFromFile(_gl, _logger, path);
            _shaders[path] = newResult;

            return newResult;
        }
    }
}
