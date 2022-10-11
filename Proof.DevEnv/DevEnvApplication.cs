using Proof.Core;
using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System;
using System.Diagnostics;

namespace Proof.DevEnv
{
    public class DevEnvApplication : Application
    {
        public DevEnvApplication(ScriptLoader scriptLoader)
            : base(
                new NoLogger(),
                "DevEnv",
                false,
                scriptLoader,
                true,
                GetParentWindow())
        { }

        private static IntPtr GetParentWindow()
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
