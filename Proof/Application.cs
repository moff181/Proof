using Proof.Core.Logging;
using Proof.OpenGL;
using Proof.Render;
using Proof.Render.Buffers;

namespace Proof
{
    public abstract class Application
    {
        public void Run()
        {
            ALogger logger = GetLogger();

            try
            {
                logger.LogInfo("Application starting...");

                using var window = new Window(logger, 1280, 720, "Title", false);

                float[] vertices = { -0.5f, -0.5f, 0.5f, -0.5f, 0.0f, 0.5f };
                int[] indices = { 0, 1, 2 };

                var layout = new VertexLayout();
                layout.AddArray(2);

                using var vertexBuffer = new VertexBuffer(logger, 50000);
                using var indexBuffer = new IndexBuffer(logger, 40000);

                using var shader = new Shader(logger, "res/shaders/Static.vertex", "res/shaders/Static.frag");

                uint frames = 0;
                DateTime lastUpdate = DateTime.Now;
                while (!window.ShouldClose())
                {
                    window.Update();

                    vertexBuffer.Submit(vertices);
                    indexBuffer.Submit(indices);

                    shader.Bind();
                    vertexBuffer.Flush(layout);
                    indexBuffer.Flush();

                    frames++;
                    if((DateTime.Now - lastUpdate).TotalMilliseconds >= 1000)
                    {
                        logger.LogDebug($"{frames} fps ({Math.Round(1000.0f / frames, 2)} ms/frame)");

                        frames = 0;
                        lastUpdate = DateTime.Now;
                    }
                }
            }
            catch(Exception e)
            {
                logger.LogError("Top level exception caught", e);
            }
        }

        protected abstract ALogger GetLogger();
    }
}
