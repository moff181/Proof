using Proof.Core;
using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using Proof.Render;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InputManager = Proof.Input.InputManager;

namespace Proof.DevEnv.Components
{
    public partial class SceneEditor : UserControl
    {
        private Application? _application;
        private ModelLibrary? _modelLibrary;
        private readonly MainWindow _mainWindow;
        private readonly string _projectName;
        private readonly ChangeHistory _changeHistory;

        public SceneEditor(WindowSettings? nullableSettings, string scene, MainWindow mainWindow, string projectName)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _projectName = projectName;
            _changeHistory = new ChangeHistory(OnUnsavedChange);

            _mainWindow.Title = $"{MainWindow.ProofTitle} - {projectName}";

            LoadSettings(null);            

            Task.Run(() => ProcessGameEngine(scene));
        }

        public IEnumerable<CommandBinding> GetCommandBindings()
        {
            return Tools.GetCommandBindings();
        }

        private void OnUnsavedChange()
        {
            _mainWindow.Title = $"* {GetTitle()}";
        }

        private string GetTitle()
        {
            return $"{MainWindow.ProofTitle} - {_projectName}";
        }

        private void LoadSettings(WindowSettings? nullableSettings)
        {
            if (nullableSettings != null)
            {
                WindowSettings settings = nullableSettings.Value;

                MainGrid.ColumnDefinitions.Clear();

                var c0 = new ColumnDefinition();
                c0.Width = new GridLength(settings.LeftPanelWidth / (float)settings.Width, GridUnitType.Star);
                var c1 = new ColumnDefinition();
                c1.Width = new GridLength((settings.Width - settings.LeftPanelWidth - settings.RightPanelWidth) / (float)settings.Width, GridUnitType.Star);
                var c2 = new ColumnDefinition();
                c2.Width = new GridLength(settings.RightPanelWidth / (float)settings.Width, GridUnitType.Star);

                MainGrid.ColumnDefinitions.Add(c0);
                MainGrid.ColumnDefinitions.Add(c1);
                MainGrid.ColumnDefinitions.Add(c2);
            }
        }

        private void ProcessGameEngine(string scene)
        {
            string gameDllPath = Path.Combine(Directory.GetCurrentDirectory(), "Game.dll");
            var assemblyWrapper = new AssemblyWrapper(gameDllPath);
            var scriptLoader = new ScriptLoader(assemblyWrapper, new NoLogger());

            _application = new DevEnvApplication(scriptLoader);
            SizeGameWindowToEditorWindow();

            Task.Run(() =>
            {
                while (_application.Scene == null)
                {
                    // Poll for scene to be loaded
                }

                Tools.Init(_application.Scene, assemblyWrapper, this);
                _modelLibrary = new ModelLibrary(new NoLogger());
                CreateSidePanels(scriptLoader, _application.Window.BuildInputManager());
            });

            _application.Run(scene);
        }

        private void CreateSidePanels(ScriptLoader scriptLoader, InputManager inputManager)
        {
            if(_application?.Scene == null || _modelLibrary == null)
            {
                return;
            }

            Dispatcher.Invoke(
                () =>
                    LeftPanel.Init(
                        _application.Scene,
                        e => RightPanel.Init(
                            _application.Scene,
                            e,
                            _modelLibrary,
                            scriptLoader,
                            inputManager,
                            () => CreateSidePanels(scriptLoader, inputManager))));
        }

        private void GridSplitter_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            SizeGameWindowToEditorWindow();
        }

        private void SizeGameWindowToEditorWindow()
        {
            var window = _application?.Window;

            if (window == null)
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                double leftPanelWidth = LeftPanel.ActualWidth;
                double width = GameDisplayPanel.ActualWidth - RightSplitter.ActualWidth;

                _application?.GlQueue.Enqueue(() =>
                {
                    window.Resize((int)width, (int)(width * 9.0f / 16.0f));
                    window.Move((int)leftPanelWidth, (int)Tools.ActualHeight);
                });
            });
        }
    }
}
