using Proof.Core.Logging;
using Proof.Entities;
using Proof.Entities.Components;
using Proof.OpenGL;
using Proof.Render;
using Proof.Render.Buffers;
using System.Numerics;

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

                using var window = new Window(logger, 1280, 720, GetTitle(), false);

                var modelLibrary = new ModelLibrary(logger);
                using Model? model = modelLibrary.Get("res/models/Square.model");

                var layout = VertexLayout.LoadFromFile(logger, "res/layouts/Static.xml");
                using var shader = Shader.LoadFromFile(logger, "res/shaders/Static.xml");

                using var renderer = new Renderer(logger, 50000, 40000);

                using var scene = new Scene(logger);

                var camera = new Entity();
                camera.AddComponent(new CameraComponent(shader));
                camera.AddComponent(new TransformComponent(new Vector2(0, 0), new Vector2(1, 1)));
                scene.AddEntity(camera);

                var entity = new Entity();
                if (model != null)
                {
                    entity.AddComponent(new RenderableComponent(renderer, layout, model));
                }
                entity.AddComponent(new TransformComponent(new Vector2(0.0f, 0.0f), new Vector2(1, 1)));
                scene.AddEntity(entity);

                uint frames = 0;
                DateTime lastUpdate = DateTime.Now;
                while (!window.ShouldClose())
                {
                    window.Update();

                    shader.Bind();

                    scene.Update();

                    renderer.Flush(layout);

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
        protected abstract string GetTitle();
    }
}
