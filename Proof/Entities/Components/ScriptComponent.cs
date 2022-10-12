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

        public bool IsLoaded()
        {
            return _script != null;
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

        public object? GetProperty(string name)
        {
            if(_script == null)
            {
                return null;
            }

            Type type = _script.GetType();
            var property = type.GetProperty(name);
            if(property == null)
            {
                return null;
            }

            return property.GetValue(_script);
        }

        public bool SetProperty(string name, string value)
        {
            if(_script == null)
            {
                return false;
            }

            Type type = _script.GetType();
            var property = type.GetProperty(name);
            if(property == null)
            {
                return false;
            }

            var propertyType = property.PropertyType;
            if(propertyType == typeof(int))
            {
                property.SetValue(_script, int.Parse(value));
            }
            else if (propertyType == typeof(long))
            {
                property.SetValue(_script, long.Parse(value));
            }
            else if (propertyType == typeof(float))
            {
                property.SetValue(_script, float.Parse(value));
            }
            else if (propertyType == typeof(double))
            {
                property.SetValue(_script, double.Parse(value));
            }
            else if (propertyType == typeof(bool))
            {
                property.SetValue(_script, bool.Parse(value));
            }
            else if (propertyType == typeof(char))
            {
                property.SetValue(_script, char.Parse(value));
            }
            else if (propertyType == typeof(byte))
            {
                property.SetValue(_script, byte.Parse(value));
            }
            else if (propertyType == typeof(short))
            {
                property.SetValue(_script, short.Parse(value));
            }
            else if (propertyType == typeof(string))
            {
                property.SetValue(_script, value);
            }
            else
            {
                return false;
            }

            return true;
        }

        public XElement ToXml()
        {
            var propertiesNode = new XElement("Properties");

            foreach(KeyValuePair<string, object?> property in GetProperties())
            {
                propertiesNode.Add(
                    new XElement("Property",
                        new XElement("Name", property.Key),
                        new XElement("Value", property.Value)));
            }

            return new XElement(
                "ScriptComponent",
                new XElement("Class", ScriptName),
                propertiesNode);
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
