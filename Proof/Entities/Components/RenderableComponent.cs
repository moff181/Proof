using Proof.Core.Logging;
using Proof.Render;
using Proof.Render.Buffers;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class RenderableComponent : IComponent
    {
        private readonly Renderer _renderer;
        private readonly VertexLayout _layout;

        private readonly Model _model;
        private readonly int _layer;

        private float[] _verticesBuffer;
        private Vector2 _previousPosition;
        private Vector2 _previousScale;

        public RenderableComponent(Renderer renderer, VertexLayout layout, Model model, int layer)
        {
            _renderer = renderer;
            _layout = layout;
            _model = model;
            _layer = layer;

            _verticesBuffer = new float[model.Vertices.Length];
            Array.Copy(_model.Vertices, _verticesBuffer, _verticesBuffer.Length);

            _previousPosition = Vector2.Zero;
            _previousScale = Vector2.One;
        }

        public void Update(Entity entity)
        {
            var transformComponent = entity.GetComponent<TransformComponent>();
            if (transformComponent != null && (transformComponent.Position != _previousPosition || transformComponent.Scale != _previousScale))
            {
                Array.Copy(_model.Vertices, _verticesBuffer, _verticesBuffer.Length);

                for (int i = _layout.PositionIndex; i < _verticesBuffer.Length; i += _layout.SumOfElements())
                {
                    _verticesBuffer[i] = transformComponent.Scale.X * _verticesBuffer[i] + transformComponent.Position.X;
                    _verticesBuffer[i+1] = transformComponent.Scale.Y * _verticesBuffer[i+1] + transformComponent.Position.Y;
                }
            }

            _renderer.Submit(_verticesBuffer, _model.Indices, _layer);
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
