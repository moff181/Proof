using Proof.Core.Logging;
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
            _entities.ForEach(e => e.Update());
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
            string filePath)
        {
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
                logger.LogWarn("Could not find Entities node while loading shader. Assuming no elements in scene.");
                return scene;
            }

            var entities = entitiesNode.Elements("Entity");
            foreach (XElement entityNode in entities)
            {
                Entity entity = Entity.LoadFromNode(logger, shader, modelLibrary, renderer, layout, entityNode);
                scene.AddEntity(entity);
            }

            return scene;
        }
    }
}
