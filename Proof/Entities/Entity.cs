﻿using Proof.Core.Logging;
using Proof.Entities.Components;
using Proof.Entities.Components.ScriptLoaders;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using System.Xml.Linq;

namespace Proof.Entities
{
    public class Entity
    {
        private readonly Dictionary<Type, IComponent> _components;

        public Entity()
        {
            _components = new Dictionary<Type, IComponent>();
        }

        public void Update()
        {
            foreach (IComponent current in _components.Values)
            {
                current.Update(this);
            }
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component.GetType(), component);
        }

        public T? GetComponent<T>()
        {
            return (T)_components[typeof(T)];
        }

        public static Entity LoadFromNode(
            ALogger logger,
            Shader shader,
            ModelLibrary modelLibrary,
            Renderer renderer,
            VertexLayout layout,
            InputManager inputManager,
            IScriptLoader scriptLoader,
            XElement node)
        {
            var entity = new Entity();

            XElement? componentsNode = node.Element("Components");
            if(componentsNode == null)
            {
                logger.LogWarn("Could not find Components node while loading entity. Creating with no components.");
                return entity;
            }

            var componentLoader = new ComponentLoader(logger);
            foreach (XElement componentNode in componentsNode.Elements())
            {
                entity.AddComponent(
                    componentLoader.LoadFromNode(
                        shader,
                        modelLibrary,
                        renderer,
                        layout,
                        inputManager,
                        scriptLoader,
                        componentNode));
            }

            return entity;
        }
    }
}
