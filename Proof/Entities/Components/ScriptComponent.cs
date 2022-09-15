﻿using Proof.Entities.Components.Scripts;
using Proof.Input;
using System.Xml;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class ScriptComponent : IComponent
    {
        private readonly string _scriptName;
        private readonly IScript _script;

        public ScriptComponent(string scriptName, IScript script)
        {
            _scriptName = scriptName;
            _script = script;
        }

        public void Update(Entity entity)
        {
            _script.Update(entity);
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