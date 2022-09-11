using Proof.Input;
using System.Xml.Linq;

namespace Proof.Entities.Components.Scripts
{
    public class NoScriptLoader : IScriptLoader
    {
        public IScript LoadScriptComponent(string className, XElement componentNode, InputManager inputManager)
        {
            return new NoScript();
        }
    }
}
