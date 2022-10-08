﻿using Proof.DevEnv.Components.EntityComponents;
using Proof.Entities;
using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using Proof.Render;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Proof.DevEnv.Components
{
    public partial class ComponentsPanel : UserControl
    {
        private Scene? _scene;
        private Entity? _entity;
        private ModelLibrary? _modelLibrary;
        private Action? _refresh;

        public ComponentsPanel()
        {
            InitializeComponent();

            UpdateVisibility(Visibility.Collapsed);
        }

        public void Init(Scene scene, Entity entity, ModelLibrary modelLibrary, Action refresh)
        {
            _scene = scene;
            _entity = entity;
            _modelLibrary = modelLibrary;
            _refresh = refresh;

            Body.Children.Clear();

            UpdateVisibility(Visibility.Visible);

            foreach (IComponent comp in entity.GetComponents())
            {
                UIElement uiElement = comp switch
                {
                    CameraComponent cameraComp => new CameraComponentPanel(cameraComp),
                    RenderableComponent renderableComp => new RenderableComponentPanel(renderableComp, modelLibrary),
                    ScriptComponent scriptComp => new ScriptComponentPanel(scriptComp),
                    TransformComponent transformComp => new TransformComponentPanel(transformComp),
                    _ => new TextBlock { Text = comp.GetType().Name }
                };

                Body.Children.Add(uiElement);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (_scene == null || _entity == null || _refresh == null)
            {
                return;
            }

            _scene.Entities.Remove(_entity);
            UpdateVisibility(Visibility.Collapsed);
            _refresh();
        }

        private void AddComponent_Click(object sender, RoutedEventArgs e)
        {
            object? selectedItem = NewComponentList.SelectedValue;
            if(selectedItem == null)
            {
                return;
            }

            ComboBoxItem? comboBoxItem = selectedItem as ComboBoxItem;

            string? componentLabel = comboBoxItem?.Content?.ToString();
            if(componentLabel == null)
            {
                return;
            }

            if (_entity == null || _scene == null || _modelLibrary == null || _refresh == null)
            {
                return;
            }

            switch(componentLabel)
            {
                case "Camera Component":
                    _entity.AddComponent(
                        new CameraComponent(
                            _scene.Shader,
                            !_scene.Entities
                                .SelectMany(x => x.GetComponents())
                                .Any(x => x is CameraComponent y && y.Active)));
                    break;
                case "Renderable Component":
                    _entity.AddComponent(
                        new RenderableComponent(
                            _scene.Renderer,
                            _scene.Shader.GetLayout(),
                            _modelLibrary.Get("res/models/Square.model") ?? throw new IOException("Could not find Square.model"),
                            0));
                    break;
                case "Script Component":
                    _entity.AddComponent(new ScriptComponent("Please enter a script name.", new NoScript()));
                    break;
                case "Transform Component":
                    _entity.AddComponent(new TransformComponent());
                    break;
                default:
                    return;
            }

            Init(_scene, _entity, _modelLibrary, _refresh);
        }

        private void UpdateVisibility(Visibility visibility)
        {
            Remove.Visibility = visibility;
            AddComponent.Visibility = visibility;
            NewComponentList.Visibility = visibility;
        }
    }
}
