using Proof.Core.Logging;

namespace Proof.Render
{
    internal class Model : IDisposable
    {
        private readonly ALogger _logger;

        public Model(ALogger logger, float[] vertices, int[] indices)
        {
            _logger = logger;
            Vertices = vertices;
            Indices = indices;

            _logger.LogInfo("Model created.");
        }

        public float[] Vertices { get; }

        public int[] Indices { get; }

        public void Dispose()
        {
            _logger.LogInfo("Model disposed of.");
        }
    }
}
