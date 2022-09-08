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

            Task.Run(ProcessGameEngine);
        }

        private void ProcessGameEngine()
        {
            // This will need updating to not just hardcode
            Directory.SetCurrentDirectory("../../../../Sandbox");

            var application = new DevEnvApplication();
            _window = application.Window;

            application.Run("res/scenes/TestScene.xml");
        }
    }
}
