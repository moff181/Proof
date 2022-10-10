using Proof.Entities.Components.Scripts;
using Proof.Input;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class ScriptComponent : IComponent
    {
        public string ScriptName { get; set; }

        private readonly IScript _script;

        public ScriptComponent(string scriptName, IScript script)
        {
            ScriptName = scriptName;
            _script = script;
        }

        public void Update(Entity entity)
        {
            if(!Application.ScriptsEnabled)
            {
                return;
            }

            _script.Update(entity);
        }

        public XElement ToXml()
        {
            return new XElement(
                "ScriptComponent",
                new XElement("Class", ScriptName));
        }

        public static ScriptComponent LoadFromXml(XElement componentNode, IScriptLoader scriptLoader, InputManager inputManager)
        {
            XElement? classNode = componentNode.Element("Class");
            if (classNode == null)
            {
                throw new XmlException("Could not find Class node in ScriptComponent.");
            }

            string className = classNode.Value;
            IScript script = scriptLoader.LoadScriptComponent(className, componentNode, inputManager);

            return new ScriptComponent(className, script);
        }
    }
}
