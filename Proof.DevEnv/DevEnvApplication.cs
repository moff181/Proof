using Proof.Core.Logging;
using Proof.Entities.Components.ScriptLoaders;
using System;
using System.Diagnostics;

namespace Proof.DevEnv
{
    public class DevEnvApplication : Application
    {
        protected override ALogger GetLogger()
        {
            return new NoLogger();
        }

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
            { }

            return windowHandle;
        }
    }
}
