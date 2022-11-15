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

            _textureId = _gl.GenTextures(1)[0];
            _gl.BindTexture(GLConstants.GL_TEXTURE_2D, _textureId);

            _gl.TexParameteri(GLConstants.GL_TEXTURE_2D, GLConstants.GL_TEXTURE_MIN_FILTER, GLConstants.GL_LINEAR);
            _gl.TexParameteri(GLConstants.GL_TEXTURE_2D, GLConstants.GL_TEXTURE_MAG_FILTER, GLConstants.GL_LINEAR);
            _gl.TexParameteri(GLConstants.GL_TEXTURE_2D, GLConstants.GL_TEXTURE_WRAP_S, GLConstants.GL_CLAMP_TO_EDGE);
            _gl.TexParameteri(GLConstants.GL_TEXTURE_2D, GLConstants.GL_TEXTURE_WRAP_T, GLConstants.GL_CLAMP_TO_EDGE);

            PngImage image = PngLoader.LoadImage(filePath);

            IntPtr ptr = Marshal.AllocHGlobal(image.Data.Length);
            Marshal.Copy(image.Data, 0, ptr, image.Data.Length);

            _gl.TexImage2D(
                GLConstants.GL_TEXTURE_2D,
                0,
                GLConstants.GL_RGB8,
                image.CoreImageData.Width,
                image.CoreImageData.Height,
                0,
                GLConstants.GL_RGB,
                GLConstants.GL_UNSIGNED_BYTE,
                ptr);

            logger.LogInfo($"Loaded texture successfully.");
        }

        public string FilePath { get; set; }

        public void Bind(int textureSlot)
        {
            _gl.ActiveTexture(GLConstants.GL_TEXTURE0 + textureSlot);
            _gl.BindTexture(GLConstants.GL_TEXTURE_2D, _textureId);
        }
    }
}
