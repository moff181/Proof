using Proof.Core.Logging;
using Proof.Input;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components.ScriptLoaders
{
    public class ScriptLoader : IScriptLoader
    {
        private readonly ALogger _logger;

        public ScriptLoader(ALogger logger)
        {
            _logger = logger;
        }

        public IComponent LoadScriptComponent(XElement componentNode, InputManager inputManager)
        {
            XElement? classNode = componentNode.Element("Class");
            if (classNode == null)
            {
                throw new XmlException("Could not find Class node in ScriptElement.");
            }

            string? className = classNode.Value;

            Assembly? assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                throw new EntryPointNotFoundException("Could not find entry assembly while loading ScriptComponent");
            }

            Type? t = assembly.GetType(className);
            if (t == null)
            {
                throw new TypeLoadException($"Could not load type specified in ScriptComponent: {className}");
            }

            if (!t.GetInterfaces().Contains(typeof(IComponent)))
            {
                throw new TypeLoadException($"Class specified in ScriptComponent was invalid as it didn't implement IComponent: {className}");
            }

            IComponent component = CreateInstance(t, inputManager);

            SetProperties(t, component, componentNode);

            return component;
        }

        private IComponent CreateInstance(Type t, InputManager inputManager)
        {
            foreach (ConstructorInfo constructorInfo in t.GetConstructors())
            {
                if (!constructorInfo.IsPublic)
                {
                    continue;
                }

                ParameterInfo[] paramInfos = constructorInfo.GetParameters();
                var parameters = new List<object>();
                foreach (ParameterInfo parameter in paramInfos)
                {
                    if (parameter.ParameterType == typeof(InputManager))
                    {
                        parameters.Add(inputManager);
                    }
                }

                if (parameters.Count() != paramInfos.Length)
                {
                    continue;
                }

                return (IComponent)constructorInfo.Invoke(parameters.ToArray());
            }

            throw new TypeLoadException($"Could not find a suitable constructor for type specified in ScriptComponent: {t.FullName}");
        }

        private void SetProperties(Type t, IComponent component, XElement componentNode)
        {
            Dictionary<string, string> providedValues = ExtractProvidedPropertyValues(componentNode);
            if (providedValues == null || !providedValues.Any())
            {
                return;
            }

            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                if (propertyInfo.SetMethod == null)
                {
                    _logger.LogWarn($"Found property while loading ScriptComponent that had no setter: {propertyInfo.Name} in {t.FullName}");
                    continue;
                }

                if (!providedValues.TryGetValue(propertyInfo.Name, out string? value))
                {
                    continue;
                }

                if (value == null)
                {
                    continue;
                }

                object? valueParsed = ParsePropertyValue(propertyInfo, value);
                if (valueParsed == null)
                {
                    continue;
                }

                propertyInfo.SetValue(component, valueParsed);
            }
        }

        private Dictionary<string, string> ExtractProvidedPropertyValues(XElement componentNode)
        {
            XElement? propertiesNode = componentNode.Element("Properties");
            if (propertiesNode == null)
            {
                return new Dictionary<string, string>();
            }

            var providedValues = new Dictionary<string, string>();
            foreach (XElement providedValueNode in propertiesNode.Elements("Property"))
            {
                XElement? propertyNameNode = providedValueNode.Element("Name");
                if (propertyNameNode == null)
                {
                    _logger.LogWarn("Property node was found while loading ScriptComponent with no Name node provided. Skipping.");
                    continue;
                }

                XElement? propertyValueNode = providedValueNode.Element("Value");
                if (propertyValueNode == null)
                {
                    _logger.LogWarn("Property node was found while loading ScriptComponent with no Value node provided. Skipping.");
                    continue;
                }

                providedValues.Add(propertyNameNode.Value, propertyValueNode.Value);
            }

            return providedValues;
        }

        private object? ParsePropertyValue(PropertyInfo propertyInfo, string value)
        {
            Type type = propertyInfo.PropertyType;

            if (type == typeof(string))
            {
                return value;
            }

            if (type == typeof(char))
            {
                return char.Parse(value);
            }

            if (type == typeof(bool))
            {
                return bool.Parse(value);
            }

            if (type == typeof(byte))
            {
                return byte.Parse(value);
            }

            if (type == typeof(short))
            {
                return short.Parse(value);
            }

            if (type == typeof(int))
            {
                return int.Parse(value);
            }

            if (type == typeof(long))
            {
                return long.Parse(value);
            }

            if (type == typeof(float))
            {
                return float.Parse(value);
            }

            if (type == typeof(double))
            {
                return double.Parse(value);
            }

            _logger.LogWarn($"Invalid type for ScriptComponent property: {propertyInfo.PropertyType.FullName}");
            return null;
        }
    }
}
