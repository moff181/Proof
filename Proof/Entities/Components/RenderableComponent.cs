using Proof.Core.Logging;
using Proof.Render;
using Proof.Render.Buffers;
using Proof.Render.Renderer;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class RenderableComponent : IComponent
    {
        private readonly Renderer _renderer;
        private readonly VertexLayout _layout;

        private float[] _verticesBuffer;
        private Vector2 _previousPosition;
        private Vector2 _previousScale;

        public RenderableComponent(Renderer renderer, VertexLayout layout, Model model, int layer)
        {
            _renderer = renderer;
            _layout = layout;

            _previousPosition = Vector2.Zero;
            _previousScale = Vector2.One;

            Layer = layer;

            _verticesBuffer = Array.Empty<float>();
            Model = model;
            SetModel(model);
        }

        public Model Model { get; private set; }
        public int Layer { get; set; }

        public void SetModel(Model model)
        {
            Model = model;

            _verticesBuffer = new float[model.Vertices.Length];
            Array.Copy(Model.Vertices, _verticesBuffer, _verticesBuffer.Length);

            // Update _previousPosition to make it recalculate
            _previousPosition = new Vector2(float.MaxValue, float.MaxValue);
        }

        public void Update(Entity entity)
        {
            var transformComponent = entity.GetComponent<TransformComponent>();
            if (transformComponent != null && (transformComponent.Position != _previousPosition || transformComponent.Scale != _previousScale))
            {
                Array.Copy(Model.Vertices, _verticesBuffer, _verticesBuffer.Length);

                for (int i = _layout.PositionIndex; i < _verticesBuffer.Length; i += _layout.SumOfElements())
                {
                    _verticesBuffer[i] = transformComponent.Scale.X * _verticesBuffer[i] + transformComponent.Position.X;
                    _verticesBuffer[i+1] = transformComponent.Scale.Y * _verticesBuffer[i+1] + transformComponent.Position.Y;
                }
            }

            _renderer.Submit(_verticesBuffer, Model.Indices, Layer);
        }

        public XElement ToXml()
        {
            return new XElement(
                "RenderableComponent",
                new XElement("Model", Model.Path),
                new XElement("Layer", Layer));
        }

        public static IComponent LoadFromNode(
            ALogger logger,
            ModelLibrary modelLibrary,
            Renderer renderer,
            VertexLayout layout,
            XElement componentNode)
        {
            XElement? modelNode = componentNode.Element("Model");
            if (modelNode == null)
            {
                throw new XmlException("Could not find Model node while loading RenderableComponent.");
            }

            Model? model = modelLibrary.Get(modelNode.Value);
            if (model == null)
            {
                throw new IOException($"Could not load model from RenderableComponent: {modelNode.Value}");
            }

            XElement? layerNode = componentNode.Element("Layer");
            if(layerNode == null)
            {
                logger.LogWarn("Could not find Layer node while loading RenderableComponent. Assuming layer = 0.");
            }

            int layer = int.Parse(layerNode?.Value ?? "0");

            return new RenderableComponent(renderer, layout, model, layer);
        }
    }
}
