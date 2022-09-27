using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities
{
    public class Scene : IDisposable
    {
        private readonly ALogger _logger;
        private readonly Shader _shader;
        private readonly Renderer _renderer;

        public Scene(ALogger logger, Shader shader, Renderer renderer)
        {
            _logger = logger;
            Entities = new List<Entity>();

            _logger.LogInfo("Scene created.");
            _shader = shader;
            _renderer = renderer;
        }
        
        public List<Entity> Entities { get; }

        public void Dispose()
        {
            _logger.LogInfo("Disposing of scene...");
            _shader.Dispose();
            _logger.LogInfo("Scene disposed of.");
        }

        public void Update()
        {
            _shader.Bind();

            foreach (Entity e in Entities)
            {
                e.Update();
            }

            _renderer.Flush(_shader.GetLayout());
        }

        public void Save(string filePath)
        {
            DateTime start = DateTime.Now;
            _logger.LogInfo($"Saving scene to {filePath}");

            var scene = new XElement("Scene");

            scene.Add(new XElement("Shader", _shader.FilePath));

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
            ALogger logger,
            ModelLibrary modelLibrary,
            Renderer renderer,
            InputManager inputManager,
            IScriptLoader scriptLoader,
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

            XElement? shaderNode = root.Element("Shader");
            if(shaderNode == null)
            {
                throw new XmlException("Could not find Shader node while loading scene.");
            }

            var shader = Shader.LoadFromFile(logger, shaderNode.Value);

            var scene = new Scene(logger, shader, renderer);

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
                    shader.GetLayout(),
                    inputManager,
                    scriptLoader,
                    entityNode);

                scene.Entities.Add(entity);
            }

            logger.LogInfo($"Scene took {(DateTime.Now - start).TotalMilliseconds}ms to load.");

            return scene;
        }
    }
}
