using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System;
using System.Diagnostics;

namespace Proof.DevEnv
{
    public class DevEnvApplication : Application
    {
        public DevEnvApplication()
            : base(new NoLogger(), "DevEnv", new NoScriptLoader(), GetParentWindow())
        { }

        protected static IntPtr GetParentWindow()
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
