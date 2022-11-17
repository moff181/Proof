using Proof.Render.Buffers;
using System.Numerics;

namespace Proof.Render.Shaders
{
    public sealed class NoShader : IShader
    {
        public string FilePath { get; }

        public NoShader(string filePath)
        {
            FilePath = filePath;
        }

        public void Bind()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public VertexLayout GetLayout()
        {
            throw new NotImplementedException();
        }

        public void LoadUniform(string name, float val)
        {
            throw new NotImplementedException();
        }

        public void LoadUniform(string name, Matrix4x4 val)
        {
            throw new NotImplementedException();
        }

        public void LoadUniform(string name, Vector2 val)
        {
            throw new NotImplementedException();
        }

        public void LoadUniform(string name, Vector3 val)
        {
            throw new NotImplementedException();
        }

        public void LoadUniform(string name, Vector4 val)
        {
            throw new NotImplementedException();
        }

        public void LoadUniform(string name, int[] vals)
        {
            // Do nothing
        }

        public void PrepareTextureUniform()
        {
            throw new NotImplementedException();
        }
    }
}
