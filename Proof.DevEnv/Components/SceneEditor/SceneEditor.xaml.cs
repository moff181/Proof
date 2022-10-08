using Proof.Core.Logging;
using Proof.Render;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components
{
    public partial class SceneEditor : UserControl
    {
        private Application? _application;
        private ModelLibrary? _modelLibrary;

        public SceneEditor(WindowSettings? nullableSettings, string scene)
        {
            InitializeComponent();

            

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

            Task.Run(() => ProcessGameEngine(scene));
        }

        public IEnumerable<CommandBinding> GetCommandBindings()
        {
            return Tools.GetCommandBindings();
        }

        private void ProcessGameEngine(string scene)
        {
            _application = new DevEnvApplication();
            SizeGameWindowToEditorWindow();

            Task.Run(() =>
            {
                while (_application.Scene == null)
                {
                    // Poll for scene to be loaded
                }

                Tools.Init(_application.Scene);
                _modelLibrary = new ModelLibrary(new NoLogger());
                CreateSidePanels();
            });

            _application.Run(scene);
        }

        private void CreateSidePanels()
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
                            CreateSidePanels)));
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
