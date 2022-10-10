using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Proof.DevEnv
{
    public class DevEnvApplication : Application
    {
        public DevEnvApplication()
            : base(
                new NoLogger(),
                "DevEnv",
                false,
                new ScriptLoader(
                    Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), "Game.dll")),
                    new NoLogger()),
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
