using GLFW;
using Proof.Entities;
using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using System.Numerics;

namespace Sandbox.Scripts
{
    public class CameraScript : IScript
    {
        private readonly InputManager _inputManager;

        private DateTime _lastUpdate;

        public CameraScript(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public float Speed { get; set; }

        public void Update(Entity entity)
        {
            float timeElapsedMs = ((DateTime.Now - _lastUpdate).Ticks / 10.0f) / 100.0f;
            _lastUpdate = DateTime.Now;

            TransformComponent? transform = entity.GetComponent<TransformComponent>();
            if(transform == null)
            {
                return;
            }

            float movementY = Speed * timeElapsedMs;
            float movementX = movementY * 9.0f/16.0f;

            if (_inputManager.IsKeyDown(Keys.W))
            {
                transform.Position = new Vector2(transform.Position.X, transform.Position.Y - movementY);
            }

            if (_inputManager.IsKeyDown(Keys.S))
            {
                transform.Position = new Vector2(transform.Position.X, transform.Position.Y + movementY);
            }

            if (_inputManager.IsKeyDown(Keys.A))
            {
                transform.Position = new Vector2(transform.Position.X + movementX, transform.Position.Y);
            }

            if (_inputManager.IsKeyDown(Keys.D))
            {
                transform.Position = new Vector2(transform.Position.X - movementX, transform.Position.Y);
            }
        }
    }
}
