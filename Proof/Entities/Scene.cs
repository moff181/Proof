﻿using Proof.Audio;
using Proof.Core.Logging;
using Proof.Core.Text;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.OpenGL;
using Proof.Render;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using Proof.Render.Textures;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities
{
    public sealed class Scene : IDisposable
    {
        private readonly ALogger _logger;

        public Scene(ALogger logger, IRenderer renderer, string scenePath)
        {
            _logger = logger;
            Entities = new List<Entity>();

            _logger.LogInfo("Scene created.");
            Renderer = renderer;
            FilePath = scenePath;
        }

        public List<Entity> Entities { get; }

        public IRenderer Renderer { get; }

        public string FilePath { get; }

        public void Dispose()
        {
            _logger.LogInfo("Scene disposed of.");
        }

        public void Update()
        {
            foreach (Entity e in Entities)
            {
                e.Update();
            }
            
            Renderer.Flush();
        }

        public void Save(string filePath)
        {
            DateTime start = DateTime.Now;
            _logger.LogInfo($"Saving scene to {filePath}");

            var scene = new XElement("Scene");
            var entities = new XElement("Entities");

            foreach(Entity e in Entities)
            {
                entities.Add(e.ToXml());
            }

            scene.Add(entities);
            scene.Save(filePath);
            
            _logger.LogInfo($"Finished saving scene ({(DateTime.Now - start).TotalMilliseconds}ms)");
        }

        public static Scene LoadFromFile(
            IOpenGLApi gl,
            ALogger logger,
            ShaderLibrary shaderLibrary,
            ModelLibrary modelLibrary,
            Renderer renderer,
            InputManager inputManager,
            ScriptLoader scriptLoader,
            SoundLibrary soundLibrary,
            TextureLibrary textureLibrary,
            FontLibrary fontLibrary,
            string filePath)
        {
            DateTime start = DateTime.Now;
            logger.LogInfo($"Loading scene from {filePath}...");

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException($"Could not find scene file: {filePath}");
            }

            XDocument doc = XDocument.Load(filePath);
            
            XElement? root = doc.Root;
            if (root == null)
            {
                throw new XmlException("Could not find root node while loading scene.");
            }

            var scene = new Scene(logger, renderer, filePath);

            XElement? entitiesNode = root.Element("Entities");
            if (entitiesNode == null)
            {
                logger.LogWarn("Could not find Entities node while loading scene. Assuming no entities in scene.");
                return scene;
            }

            var entities = entitiesNode.Elements("Entity");
            foreach (XElement entityNode in entities)
            {
                Entity entity = Entity.LoadFromNode(
                    logger,
                    shaderLibrary,
                    modelLibrary,
                    renderer,
                    inputManager,
                    scriptLoader,
                    soundLibrary,
                    textureLibrary,
                    fontLibrary,
                    entityNode);

                scene.Entities.Add(entity);
            }

            logger.LogInfo($"Scene took {(DateTime.Now - start).TotalMilliseconds}ms to load.");

            return scene;
        }
    }
}
