using Proof.Core.Images;
using Proof.Core.Logging;
using Proof.OpenGL;
using System.Runtime.InteropServices;

namespace Proof.Render.Textures
{
    public sealed class Texture : IDisposable
    {
        private readonly ALogger _logger;
        private readonly uint _textureId;

        public unsafe Texture(ALogger logger, string filePath)
		{
            _logger = logger;
			FilePath = filePath;

            logger.LogInfo($"Loading texture from file: {filePath}");

			_textureId = GL.glGenTextures(1)[0];
			GL.glBindTexture(GL.GL_TEXTURE_2D, _textureId);

            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_CLAMP_TO_EDGE);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_CLAMP_TO_EDGE);

            PngImage image = PngLoader.LoadImage(filePath);

            IntPtr ptr = Marshal.AllocHGlobal(image.Data.Length);
            Marshal.Copy(image.Data, 0, ptr, image.Data.Length);
            
            GL.glTexImage2D(
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

        ~Texture()
        {
            Dispose();
        }

        public void Dispose()
        {
            _logger.LogInfo($"Disposing of texture: {FilePath}...");

            GL.glDeleteTexture(_textureId);
            GC.SuppressFinalize(this);

            _logger.LogInfo("Texture disposed of.");
        }

        public string FilePath { get; set; }

        public void Bind(int textureSlot)
        {
            GL.glActiveTexture(GL.GL_TEXTURE0 + textureSlot);
            GL.glBindTexture(GL.GL_TEXTURE_2D, _textureId);
        }
    }
}
