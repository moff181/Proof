using Proof.DevEnv.Components.EntityComponents;
using Proof.Entities;
using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Proof.DevEnv.Components
{
    public partial class ComponentsPanel : UserControl
    {
        private Scene? _scene;
        private Entity? _entity;
        private ModelLibrary? _modelLibrary;
        private ScriptLoader? _scriptLoader;
        private InputManager? _inputManager;
        private ChangeHistory? _changeHistory;
        private Action? _refresh;

        public ComponentsPanel()
        {
            InitializeComponent();

            UpdateVisibility(Visibility.Collapsed);
        }

        public void Init(
            Scene scene,
            Entity entity,
            ModelLibrary modelLibrary,
            ScriptLoader scriptLoader,
            InputManager inputManager,
            ChangeHistory changeHistory,
            Action refresh)
        {
            _scene = scene;
            _entity = entity;
            _modelLibrary = modelLibrary;
            _scriptLoader = scriptLoader;
            _inputManager = inputManager;
            _changeHistory = changeHistory;
            _refresh = refresh;

            Body.Children.Clear();

            UpdateVisibility(Visibility.Visible);

            foreach (IComponent comp in entity.GetComponents())
            {
                UIElement uiElement = comp switch
                {
                    CameraComponent cameraComp => new CameraComponentPanel(cameraComp, _changeHistory, RemoveComponent),
                    ColourComponent colourComp => new ColourComponentPanel(colourComp, _changeHistory, RemoveComponent),
                    RenderableComponent renderableComp => new RenderableComponentPanel(renderableComp, modelLibrary, _changeHistory, RemoveComponent),
                    ScriptComponent scriptComp => new ScriptComponentPanel(scriptComp, RemoveComponent),
                    TransformComponent transformComp => new TransformComponentPanel(transformComp, _changeHistory, RemoveComponent),
                    _ => new TextBlock { Text = comp.GetType().Name }
                };

                Body.Children.Add(uiElement);
            }
        }

        private void RemoveComponent(IComponent component)
        {
            if(_entity == null || _scene == null || _modelLibrary == null || _refresh == null || _scriptLoader == null || _inputManager == null)
            {
                return;
            }

            _changeHistory?.RegisterChange();
            _entity.RemoveComponent(component);
            Init(_scene, _entity, _modelLibrary, _scriptLoader, _inputManager, _changeHistory, _refresh);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (_scene == null || _entity == null || _refresh == null)
            {
                return;
            }

            _changeHistory?.RegisterChange();
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

            if (_entity == null || _scene == null || _modelLibrary == null || _refresh == null || _scriptLoader == null || _inputManager == null)
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
                case "Colour Component":
                    _entity.AddComponent(new ColourComponent(new Vector3(1, 1, 1)));
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
                    _entity.AddComponent(new ScriptComponent("Please enter a script name.", _scriptLoader, _inputManager, new XElement("Temp")));
                    break;
                case "Transform Component":
                    _entity.AddComponent(new TransformComponent());
                    break;
                default:
                    return;
            }

            _changeHistory?.RegisterChange();
            Init(_scene, _entity, _modelLibrary, _scriptLoader, _inputManager, _changeHistory, _refresh);
        }

        private void UpdateVisibility(Visibility visibility)
        {
            Remove.Visibility = visibility;
            AddComponent.Visibility = visibility;
            NewComponentList.Visibility = visibility;
        }
    }
}
