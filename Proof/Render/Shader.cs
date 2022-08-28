using Proof.Core.Logging;
using Proof.OpenGL;

namespace Proof.Render
{
    internal class Shader : IDisposable
    {
        private enum ShaderType
        {
            Vertex = GL.GL_VERTEX_SHADER,
            Fragment = GL.GL_FRAGMENT_SHADER,
        }

        private readonly ALogger _logger;
        private readonly uint _programId;

        public Shader(ALogger logger, string vertexFile, string fragmentFile)
        {
            _logger = logger;

            _logger.LogInfo("Creating shader...");

            uint vertexShaderId = CreateShader(_logger, ShaderType.Vertex, vertexFile);
            uint fragmentShaderId = CreateShader(_logger, ShaderType.Fragment, fragmentFile);

            _programId = GL.glCreateProgram();
            GL.glAttachShader(_programId, vertexShaderId);
            GL.glAttachShader(_programId, fragmentShaderId);

            GL.glLinkProgram(_programId);

            GL.glValidateProgram(_programId);

            GL.glDeleteShader(vertexShaderId);
            GL.glDeleteShader(fragmentShaderId);

            _logger.LogInfo("Shader created");            
        }

        public void Dispose()
        {
            _logger.LogInfo("Disposing of shader...");

            GL.glDeleteProgram(_programId);

            _logger.LogInfo("Shader disposed of.");
        }

        public void Bind()
        {
            GL.glUseProgram(_programId);
        }

        private static uint CreateShader(ALogger logger, ShaderType type, string filePath)
        {
            logger.LogInfo($"Creating {type} shader from {filePath}...");

            string src;
            try
            {
                src = File.ReadAllText(filePath);
            }
            catch(Exception e)
            {
                throw new Exception($"Could not read shader file from {filePath}", e);
            }

            uint shaderId = GL.glCreateShader((int)type);

            GL.glShaderSource(shaderId, src);
            GL.glCompileShader(shaderId);

            string shaderLog = GL.glGetShaderInfoLog(shaderId);
            if (!string.IsNullOrWhiteSpace(shaderLog))
            {
                logger.LogWarn(shaderLog);
            }

            return shaderId;
        }
    }
}
