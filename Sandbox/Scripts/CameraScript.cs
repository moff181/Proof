using Proof.Entities;
using Proof.Entities.Components;
using Proof.Input;

namespace Sandbox.Scripts
{
    public class CameraScript : IComponent
    {
        private readonly InputManager _inputManager;

        public CameraScript(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public void Update(Entity entity)
        {
            
        }
    }
}
