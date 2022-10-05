using Proof.Render.Buffers;
using System.Numerics;

namespace Proof.Render.Shaders
{
    public interface IShader
    {
        string FilePath { get; }

        void Bind();
        void Dispose();
        VertexLayout GetLayout();
        void LoadUniform(string name, float val);
        void LoadUniform(string name, Matrix4x4 val);
        void LoadUniform(string name, Vector2 val);
        void LoadUniform(string name, Vector3 val);
        void LoadUniform(string name, Vector4 val);
    }
}