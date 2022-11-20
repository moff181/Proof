using Proof.Audio;
using Proof.Core.Logging;
using Proof.Core.Text;
using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.OpenGL;
using Proof.Render;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using Proof.Render.Textures;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities
{
    public sealed class Scene : IDisposable
    {
        private readonly ALogger _logger;

        public Scene(ALogger logger, IShader shader, IRenderer renderer, string scenePath)
        {
            _logger = logger;
            Entities = new List<Entity>();

            _logger.LogInfo("Scene created.");
            Shader = shader;
            Renderer = renderer;
            FilePath = scenePath;
        }

        public List<Entity> Entities { get; }

        public IShader Shader { get; }

        public IRenderer Renderer { get; }

        public string FilePath { get; }

        public void Dispose()
        {
            _logger.LogInfo("Disposing of scene...");
            Shader.Dispose();
            _logger.LogInfo("Scene disposed of.");
        }

        public void Update()
        {
            Shader.Bind();

            foreach (Entity e in Entities)
            {
                e.Update();
            }
            
            Renderer.Flush(Shader.GetLayout());
        }

        public void Save(string filePath)
        {
            DateTime start = DateTime.Now;
            _logger.LogInfo($"Saving scene to {filePath}");

            var scene = new XElement("Scene");

            scene.Add(
                new XElement(
                    "Shaders",
                    new XElement(
                        "Shader",
                        Shader.FilePath)));

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
            ModelLibrary modelLibrary,
            Renderer renderer,
            InputManager inputManager,
            ScriptLoader scriptLoader,
            SoundLibrary soundLibrary,
            TextureLibrary textureLibrary,
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

            XElement? shadersNode = root.Element("Shaders");
            if (shadersNode == null)
            {
                throw new XmlException("Could not find Shaders node while loading scene.");
            }

            var shaders = new List<Shader>();
            foreach (var shaderNode in shadersNode.Elements("Shader"))
            {
                shaders.Add(Render.Shaders.Shader.LoadFromFile(gl, logger, shaderNode.Value));
            }

            var shader = shaders.First();
            var scene = new Scene(logger, shader, renderer, filePath);

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
                    shaders.ToArray(),
                    modelLibrary,
                    renderer,
                    inputManager,
                    scriptLoader,
                    soundLibrary,
                    textureLibrary,
                    entityNode);

                scene.Entities.Add(entity);
            }

            var textTest = new Entity("TextTest");
            textTest.AddComponent(new TextComponent(
                "yello, world!",
                Font.LoadFromFile("res/fonts/calibri.fnt", textureLibrary),
                renderer,
                modelLibrary.Get("res/models/UISquare.model"),
                1,
                shaders[1]));
            textTest.AddComponent(new TransformComponent(new Vector2(-1, 0), new Vector2(1, 1)));
            scene.Entities.Add(textTest);

            logger.LogInfo($"Scene took {(DateTime.Now - start).TotalMilliseconds}ms to load.");

            return scene;
        }
    }
}
