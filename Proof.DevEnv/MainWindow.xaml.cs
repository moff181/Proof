using Window = System.Windows.Window;
using ProofWindow = Proof.Render.Window;
using Proof.Core.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

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
            IntPtr windowHandle;
            while ((windowHandle = Process.GetCurrentProcess().MainWindowHandle) == IntPtr.Zero)
            { }

            var logger = new ConsoleLogger();
            var window = new ProofWindow(logger, 720, 576, "DevEnv", false, windowHandle);

            while(!window.ShouldClose())
            {
                window.Update();
            }
        }
    }
}
