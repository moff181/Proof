using Proof.Core.Logging;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities
{
    public class Scene : IDisposable
    {
        private readonly ALogger _logger;
        private readonly List<Entity> _entities;

        public Scene(ALogger logger)
        {
            _logger = logger;
            _entities = new List<Entity>();

            _logger.LogInfo("Scene created.");
        }

        public void Dispose()
        {
            _logger.LogInfo("Scene disposed of.");
        }

        public void Update()
        {
            foreach(Entity e in _entities)
            {
                e.Update();
            }
        }

        public void AddEntity(Entity e)
        {
            _entities.Add(e);
        }

        public static Scene LoadFromFile(
            ALogger logger,
            Shader shader,
            ModelLibrary modelLibrary,
            Renderer renderer,
            VertexLayout layout,
            InputManager inputManager,
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

            var scene = new Scene(logger);

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
                    shader,
                    modelLibrary,
                    renderer,
                    layout,
                    inputManager,
                    entityNode);

                scene.AddEntity(entity);
            }

            logger.LogInfo($"Scene took {(DateTime.Now - start).TotalMilliseconds}ms to load.");

            return scene;
        }
    }
}
