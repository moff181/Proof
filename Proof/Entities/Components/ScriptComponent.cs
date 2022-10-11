using Proof.Entities.Components.Scripts;
using Proof.Input;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class ScriptComponent : IComponent
    {
        public string ScriptName { get; set; }

        private readonly ScriptLoader _scriptLoader;
        private readonly InputManager _inputManager;
        private readonly XElement _componentNode;
        private IScript? _script;

        public ScriptComponent(string scriptName, ScriptLoader scriptLoader, InputManager inputManager, XElement componentNode)
        {
            ScriptName = scriptName;
            _scriptLoader = scriptLoader;
            _inputManager = inputManager;
            _componentNode = componentNode;
            _script = null;

            LoadScript();
        }

        public void Update(Entity entity)
        {
            if(!Application.ScriptsEnabled || _script == null)
            {
                return;
            }

            _script.Update(entity);
        }

        public void LoadScript()
        {
            _script = _scriptLoader.LoadScriptComponent(ScriptName, _componentNode, _inputManager);
        }

        public Dictionary<string, object?> GetProperties()
        {
            if(_script == null)
            {
                return new Dictionary<string, object?>();
            }

            PropertyInfo[] properties = _script.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var result = new Dictionary<string, object?>();
            foreach (PropertyInfo property in properties)
            {
                result.Add(property.Name, property.GetValue(_script));
            }

            return result;
        }

        public XElement ToXml()
        {
            return new XElement(
                "ScriptComponent",
                new XElement("Class", ScriptName));
        }

        public static ScriptComponent LoadFromXml(XElement componentNode, ScriptLoader scriptLoader, InputManager inputManager)
        {
            XElement? classNode = componentNode.Element("Class");
            if (classNode == null)
            {
                throw new XmlException("Could not find Class node in ScriptComponent.");
            }

            string className = classNode.Value;

            return new ScriptComponent(className, scriptLoader, inputManager, componentNode);
        }
    }
}
