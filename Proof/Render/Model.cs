using Proof.Core.Logging;

namespace Proof.Render
{
    public class Model : IDisposable
    {
        private readonly ALogger _logger;

        public Model(ALogger logger, string path, float[] vertices, int[] indices)
        {
            _logger = logger;

            Path = path;
            Vertices = vertices;
            Indices = indices;

            _logger.LogInfo("Model created.");
        }

        public string Path { get; }

        public float[] Vertices { get; }

        public int[] Indices { get; }

        public void Dispose()
        {
            _logger.LogInfo("Model disposed of.");
        }
    }
}
