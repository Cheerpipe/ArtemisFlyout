using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtemisFlyout.Util;

namespace ArtemisFlyout.Services
{
   public class ArtemisService
    {
        private const string ArtemisPath = @"D:\Repos\Artemis\src\Artemis.UI\bin\net5.0-windows\Artemis.UI.exe";
        private const string ArtemisParams = "--minimized --pcmr --ignore-plugin-lock";

        public static void GoHome()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/bring-to-foreground");
        }

        public static void GoWorkshop()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-workshop");
            Program.MainWindowInstance.CloseAnimated();
        }

        public static void GoSurfaceEditor()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-surface-editor");
            Program.MainWindowInstance.CloseAnimated();
        }

        public static void ShowDebugger()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-debugger");
            Program.MainWindowInstance.CloseAnimated();
        }

        public static void GoSettings()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-settings");
            Program.MainWindowInstance.CloseAnimated();
        }

        public static void LaunchArtemis()
        {
            Process.Start(ArtemisPath, ArtemisParams);
            Program.MainWindowInstance.Close();
        }
    }
}
