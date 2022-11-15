using Proof.Core.Images;
using Proof.Core.Logging;
using Proof.OpenGL;
using System.Runtime.InteropServices;

namespace Proof.Render.Textures
{
    public sealed class Texture : ITexture
    {
        private readonly IOpenGLApi _gl;

        private readonly uint _textureId;

        public unsafe Texture(IOpenGLApi gl, ALogger logger, string filePath)
        {
            _gl = gl;
            FilePath = filePath;

            logger.LogInfo($"Loading texture from file: {filePath}");

            _textureId = GL.glGenTextures(1)[0];
            _gl.BindTexture(GL.GL_TEXTURE_2D, _textureId);

            _gl.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
            _gl.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
            _gl.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_CLAMP_TO_EDGE);
            _gl.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_CLAMP_TO_EDGE);

            PngImage image = PngLoader.LoadImage(filePath);

            IntPtr ptr = Marshal.AllocHGlobal(image.Data.Length);
            Marshal.Copy(image.Data, 0, ptr, image.Data.Length);

            _gl.TexImage2D(
                GL.GL_TEXTURE_2D,
                0,
                GL.GL_RGB8,
                image.CoreImageData.Width,
                image.CoreImageData.Height,
                0,
                GL.GL_RGB,
                GL.GL_UNSIGNED_BYTE,
                ptr);

            logger.LogInfo($"Loaded texture successfully.");
        }

        public string FilePath { get; set; }

        public void Bind(int textureSlot)
        {
            _gl.ActiveTexture(GL.GL_TEXTURE0 + textureSlot);
            _gl.BindTexture(GL.GL_TEXTURE_2D, _textureId);
        }
    }
}
