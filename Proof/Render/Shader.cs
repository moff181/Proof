using Proof.Core.Logging;
using Proof.OpenGL;
using System.Numerics;

namespace Proof.Render
{
    public class Shader : IDisposable
    {
        private enum ShaderType
        {
            Vertex = GL.GL_VERTEX_SHADER,
            Fragment = GL.GL_FRAGMENT_SHADER,
        }

        private const int UniformNotFound = -1;

        private readonly ALogger _logger;
        private readonly uint _programId;

        private readonly Dictionary<string, int?> _uniformLocations;

        public Shader(ALogger logger, string vertexFile, string fragmentFile)
        {
            _logger = logger;
            _uniformLocations = new Dictionary<string, int?>();

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

            _logger.LogInfo("Shader created.");            
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

        public void LoadUniform(string name, float val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if(!uniformLocation.HasValue)
            {
                return;
            }

            GL.glUniform1f(uniformLocation.Value, val);
        }

        public void LoadUniform(string name, Vector2 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            GL.glUniform2f(uniformLocation.Value, val.X, val.Y);
        }

        public void LoadUniform(string name, Vector3 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            GL.glUniform3f(uniformLocation.Value, val.X, val.Y, val.Z);
        }

        public void LoadUniform(string name, Vector4 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            GL.glUniform4f(uniformLocation.Value, val.X, val.Y, val.Z, val.W);
        }

        public unsafe void LoadUniform(string name, Matrix4x4 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            // Assumes that values in Matrix4x4 are stored contiguously
            GL.glUniformMatrix4fv(uniformLocation.Value, 1, false, &val.M11);
        }

        private int? GetUniformLocation(string name)
        {
            if(_uniformLocations.TryGetValue(name, out int? value))
            {
                return value;
            }

            int uniformLocation = GL.glGetUniformLocation(_programId, name);
            if (uniformLocation == UniformNotFound)
            {
                _logger.LogWarn($"Could not find uniform in shader: {name}");
                _uniformLocations.Add(name, null);
                return null;
            }

            _uniformLocations.Add(name, uniformLocation);
            return uniformLocation;
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
