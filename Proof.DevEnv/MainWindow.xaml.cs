using Window = System.Windows.Window;
using ProofWindow = Proof.Render.Window;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Proof.DevEnv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Application? _application;

        public MainWindow()
        {
            InitializeComponent();

            int width = (int)Width;
            int height = (int)Height;
            Task.Run(() => ProcessGameEngine(width, height));
        }

        private void ProcessGameEngine(int width, int height)
        {
            // This will need updating to not just hardcode
            Directory.SetCurrentDirectory("../../../../Sandbox");

            _application = new DevEnvApplication();
            SizeGameWindowToEditorWindow();

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
    }
}
