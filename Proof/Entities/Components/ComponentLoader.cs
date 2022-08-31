﻿using Proof.Core.Logging;
using Proof.Render;
using System.Xml.Linq;

namespace Proof.Entities.Components
{
    public class ComponentLoader
    {
        private readonly ALogger _logger;

        public ComponentLoader(ALogger logger)
        {
            _logger = logger;
        }

        public IComponent LoadFromNode(Shader shader, XElement componentNode)
        {
            string name = componentNode.Name.LocalName;

            switch(name)
            {
                case "CameraComponent":
                    return CameraComponent.LoadFromNode(_logger, shader, componentNode);
                default:
                    throw new NotSupportedException($"Unable to component node with name: {componentNode}");
            }
        }
    }
}
