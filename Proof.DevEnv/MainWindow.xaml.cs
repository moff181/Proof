using Window = System.Windows.Window;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using Proof.Render;
using Proof.Core.Logging;
using Proof.DevEnv.Exporting;

namespace Proof.DevEnv
{
    public partial class MainWindow : Window
    {
        private const string WindowSettingsFile = "window.settings";

        private readonly string _currentDirectory;
        private Application? _application;

        public MainWindow()
        {
            InitializeComponent();

            _currentDirectory = Directory.GetCurrentDirectory();

            WindowSettings? nullableSettings = WindowSettings.Load(WindowSettingsFile);
            if(nullableSettings != null)
            {
                WindowSettings settings = nullableSettings.Value;

                WindowState = settings.Fullscreen
                    ? WindowState.Maximized
                    : WindowState.Normal;
                Width = settings.Width;
                Height = settings.Height;
                
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

            Task.Run(() => ProcessGameEngine());
        }

        private void ProcessGameEngine()
        {
            // This will need updating to not just hardcode
            Directory.SetCurrentDirectory("../../../../Sandbox");

            _application = new DevEnvApplication();
            SizeGameWindowToEditorWindow();

            Task.Run(() =>
            {
                while (_application.Scene == null)
                { 
                    // Poll for scene to be loaded
                }

                var modelLibrary = new ModelLibrary(new NoLogger());
                Dispatcher.Invoke(() => LeftPanel.Init(_application.Scene, e => RightPanel.Init(e, modelLibrary)));
            });

            _application.Run("res/scenes/TestScene.xml");
        }

        private void Window_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            SizeGameWindowToEditorWindow();
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
                        window.Move((int)leftPanelWidth, 0);
                    });
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = new WindowSettings
            {
                LeftPanelWidth = (int)LeftPanel.ActualWidth,
                RightPanelWidth = (int)RightPanel.ActualWidth,
                Fullscreen = WindowState == WindowState.Maximized,
                Width = (int)ActualWidth,
                Height = (int)ActualHeight,
            };

            settings.Save(Path.Combine(_currentDirectory, "window.settings"));

            var exporter = new Exporter(new Compiler(), new EntryPointGenerator());
            exporter.Export(Directory.GetCurrentDirectory(), "Game.dll");
            // _application?.Scene?.Save("Test.xml");
        }
    }
}
