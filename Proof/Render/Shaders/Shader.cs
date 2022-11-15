using Proof.Core.Logging;
using Proof.OpenGL;
using Proof.Render.Buffers;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Render.Shaders
{
    public sealed class Shader : IShader
    {
        private enum ShaderType
        {
            Vertex = GLConstants.GL_VERTEX_SHADER,
            Fragment = GLConstants.GL_FRAGMENT_SHADER,
        }

        private const int UniformNotFound = -1;

        private readonly IOpenGLApi _gl;
        private readonly ALogger _logger;
        private readonly uint _programId;

        private readonly Dictionary<string, int?> _uniformLocations;

        private readonly VertexLayout _vertexLayout;

        private Shader(IOpenGLApi gl, ALogger logger, string filePath, string vertexFile, string fragmentFile, VertexLayout vertexLayout)
        {
            _gl = gl;
            _logger = logger;
            _uniformLocations = new Dictionary<string, int?>();
            _vertexLayout = vertexLayout;

            FilePath = filePath;

            _logger.LogInfo("Creating shader...");

            uint vertexShaderId = CreateShader(_logger, ShaderType.Vertex, vertexFile);
            uint fragmentShaderId = CreateShader(_logger, ShaderType.Fragment, fragmentFile);

            _programId = _gl.CreateProgram();
            _gl.AttachShader(_programId, vertexShaderId);
            _gl.AttachShader(_programId, fragmentShaderId);

            _gl.LinkProgram(_programId);

            _gl.ValidateProgram(_programId);

            _gl.DeleteShader(vertexShaderId);
            _gl.DeleteShader(fragmentShaderId);

            _logger.LogInfo("Shader created.");
        }

        public string FilePath { get; }

        public void Dispose()
        {
            _logger.LogInfo("Disposing of shader...");

            _gl.DeleteProgram(_programId);

            _logger.LogInfo("Shader disposed of.");
        }

        public void Bind()
        {
            _gl.UseProgram(_programId);
        }

        public void LoadUniform(string name, float val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            _gl.Uniform1f(uniformLocation.Value, val);
        }

        public void LoadUniform(string name, Vector2 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            _gl.Uniform2f(uniformLocation.Value, val.X, val.Y);
        }

        public void LoadUniform(string name, Vector3 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            _gl.Uniform3f(uniformLocation.Value, val.X, val.Y, val.Z);
        }

        public void LoadUniform(string name, Vector4 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            _gl.Uniform4f(uniformLocation.Value, val.X, val.Y, val.Z, val.W);
        }

        public unsafe void LoadUniform(string name, Matrix4x4 val)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            // Assumes that values in Matrix4x4 are stored contiguously
            _gl.UniformMatrix4fv(uniformLocation.Value, 1, false, &val.M11);
        }

        public void LoadUniform(string name, int[] vals)
        {
            int? uniformLocation = GetUniformLocation(name);
            if (!uniformLocation.HasValue)
            {
                return;
            }

            _gl.Uniform1iv(uniformLocation.Value, vals.Length, vals);
        }

        public VertexLayout GetLayout()
        {
            return _vertexLayout;
        }

        private int? GetUniformLocation(string name)
        {
            if (_uniformLocations.TryGetValue(name, out int? value))
            {
                return value;
            }

            int uniformLocation = _gl.GetUniformLocation(_programId, name);
            if (uniformLocation == UniformNotFound)
            {
                _logger.LogWarn($"Could not find uniform in shader: {name}");
                _uniformLocations.Add(name, null);
                return null;
            }

            _uniformLocations.Add(name, uniformLocation);
            return uniformLocation;
        }

        private uint CreateShader(ALogger logger, ShaderType type, string filePath)
        {
            logger.LogInfo($"Creating {type} shader from {filePath}...");

            string src;
            try
            {
                src = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                throw new IOException($"Could not read shader file from {filePath}", e);
            }

            uint shaderId = _gl.CreateShader((int)type);

            _gl.ShaderSource(shaderId, src);
            _gl.CompileShader(shaderId);

            string shaderLog = _gl.GetShaderInfoLog(shaderId);
            if (!string.IsNullOrWhiteSpace(shaderLog))
            {
                logger.LogWarn(shaderLog);
            }

            return shaderId;
        }

        public static Shader LoadFromFile(IOpenGLApi gl, ALogger logger, string filePath)
        {
            logger.LogInfo($"Loading shader from {filePath}...");

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException($"Shader file does not exists: {filePath}");
            }

            XDocument doc = XDocument.Load(filePath);

            XElement? root = doc.Root;
            if (root == null)
            {
                throw new XmlException("Could not find root node in shader file.");
            }

            XElement? vertexFileNode = root.Element("VertexFile");
            if (vertexFileNode == null)
            {
                throw new XmlException("Could not find VertexFile node in shader file.");
            }

            XElement? fragmentFileNode = root.Element("FragmentFile");
            if (fragmentFileNode == null)
            {
                throw new XmlException("Could not find FragmentFile node in shader file.");
            }

            XElement? vertexLayoutNode = root.Element("VertexLayout");
            if (vertexLayoutNode == null)
            {
                throw new XmlException("Could not find VertexLayout node in shader file.");
            }

            VertexLayout vertexLayout = VertexLayout.LoadFromNode(logger, vertexLayoutNode);

            return new Shader(gl, logger, filePath, vertexFileNode.Value, fragmentFileNode.Value, vertexLayout);
        }
    }
}
