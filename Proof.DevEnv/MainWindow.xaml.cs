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
            SizeGameWindowToEditorWindow(width, height);

            _application.Run("res/scenes/TestScene.xml");
        }

        private void Window_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var newSize = e.NewSize;
            SizeGameWindowToEditorWindow((int)newSize.Width, (int)newSize.Height);
        }

        private void SizeGameWindowToEditorWindow(int windowWidth, int windowHeight)
        {
            var window = _application?.Window;

            if (window == null)
            {
                return;
            }

            _application?.GlQueue.Enqueue(() => 
                { 
                    window.Resize(windowWidth / 2, windowHeight / 2);
                    window.Move(windowWidth / 4, 0); 
                });
        }
    }
}
