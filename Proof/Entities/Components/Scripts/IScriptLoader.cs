using Proof.Input;
using System.Xml.Linq;

namespace Proof.Entities.Components.Scripts
{
    public interface IScriptLoader
    {
        IScript LoadScriptComponent(string className, XElement componentNode, InputManager inputManager);
    }
}
