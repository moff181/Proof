using Window = System.Windows.Window;
using System.IO;
using System.Windows;
using Proof.DevEnv.Components;

namespace Proof.DevEnv
{
    public partial class MainWindow : Window
    {
        private const string WindowSettingsFile = "window.settings";

        private readonly string _currentDirectory;
        private readonly WindowSettings? _windowSettings;

        public MainWindow()
        {
            InitializeComponent();

            _currentDirectory = Directory.GetCurrentDirectory();

            _windowSettings = WindowSettings.Load(WindowSettingsFile);
            if(_windowSettings != null)
            {
                WindowSettings settings = _windowSettings.Value;

                WindowState = settings.Fullscreen
                    ? WindowState.Maximized
                    : WindowState.Normal;
                Width = settings.Width;
                Height = settings.Height;
            }

            Content.Children.Add(new SceneEditor(_windowSettings));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var sceneEditor = (SceneEditor)Content.Children[0];

            var settings = new WindowSettings
            {
                LeftPanelWidth = (int)sceneEditor.LeftPanel.ActualWidth,
                RightPanelWidth = (int)sceneEditor.RightPanel.ActualWidth,
                Fullscreen = WindowState == WindowState.Maximized,
                Width = (int)ActualWidth,
                Height = (int)ActualHeight,
            };

            settings.Save(Path.Combine(_currentDirectory, "window.settings"));

            /*var exporter = new Exporter(
                new Compiler()
                    .WithAdditionalReferences(
                        "Proof.dll",
                        "Proof.OpenGL.dll",
                        "GLFW.NET.dll",
                        "System.Numerics.dll",
                        "System.Numerics.Vectors.dll"),
                new EntryPointGenerator("Proof Game"));
            exporter.Export(Directory.GetCurrentDirectory(), "Game.dll");
            _application?.Scene?.Save("Test.xml");*/
        }
    }
}
