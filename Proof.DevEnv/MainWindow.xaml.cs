using Window = System.Windows.Window;
using ProofWindow = Proof.Render.Window;
using Proof.Core.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Proof.DevEnv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProofWindow _window;

        public MainWindow()
        {
            InitializeComponent();

            Task.Run(ProcessGameEngine);
        }

        private void ProcessGameEngine()
        {
            // This will need to be updating to not just hardcode
            Directory.SetCurrentDirectory("../../../../Sandbox");

            var application = new DevEnvApplication();
            application.Run("res/scenes/TestScene.xml");
        }
    }
}
