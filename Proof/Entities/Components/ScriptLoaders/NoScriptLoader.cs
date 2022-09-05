using Proof.Input;
using System.Xml.Linq;

namespace Proof.Entities.Components.ScriptLoaders
{
    public class NoScriptLoader : IScriptLoader
    {
        public IComponent LoadScriptComponent(XElement componentNode, InputManager inputManager)
        {
            return new NoComponent();
        }
    }
}
