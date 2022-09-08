using Window = System.Windows.Window;
using ProofWindow = Proof.Render.Window;
using System.Threading.Tasks;
using System.IO;

namespace Proof.DevEnv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProofWindow? _window;

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

            var application = new DevEnvApplication();
            _window = application.Window;
            SizeGameWindowToEditorWindow(width, height);

            application.Run("res/scenes/TestScene.xml");
        }

        private void Window_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var newSize = e.NewSize;
            SizeGameWindowToEditorWindow((int)newSize.Width, (int)newSize.Height);
        }

        private void SizeGameWindowToEditorWindow(int windowWidth, int windowHeight)
        {
            if (_window == null)
            {
                return;
            }

            _window.Resize(windowWidth / 2, windowHeight / 2);
        }
    }
}
