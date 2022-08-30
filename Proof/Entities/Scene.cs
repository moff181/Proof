using Proof.Core.Logging;

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
    }
}
