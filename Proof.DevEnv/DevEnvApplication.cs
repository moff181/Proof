using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System;
using System.Diagnostics;

namespace Proof.DevEnv
{
    public class DevEnvApplication : Application
    {
        public DevEnvApplication()
            : base(new NoLogger())
        { }

        protected override IScriptLoader GetScriptLoader()
        {
            return new NoScriptLoader();
        }

        protected override string GetTitle()
        {
            return "DevEnv";
        }

        protected override IntPtr GetParentWindow()
        {
            IntPtr windowHandle;
            while ((windowHandle = Process.GetCurrentProcess().MainWindowHandle) == IntPtr.Zero)
            { 
                // Poll for window
            }

            return windowHandle;
        }
    }
}
