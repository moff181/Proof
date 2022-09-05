using Proof.Input;
using System.Xml.Linq;

namespace Proof.Entities.Components.ScriptLoaders
{
    public interface IScriptLoader
    {
        IComponent LoadScriptComponent(XElement componentNode, InputManager inputManager);
    }
}
