using System.Diagnostics;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Util;
using MessageBox.Avalonia.Enums;

namespace ArtemisFlyout.Services.ArtemisServices
{
    public class ArtemisService : IArtemisService
    {
        private IFlyoutService _flyoutService;
        public ArtemisService(IFlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
        }

        private const string ArtemisPath = @"D:\Repos\Artemis\src\Artemis.UI\bin\net5.0-windows\Artemis.UI.exe";
        private const string ArtemisParams = "--minimized --pcmr --ignore-plugin-lock";

        public void GoHome()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/bring-to-foreground");
        }

        public void GoWorkshop()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-workshop");
            _flyoutService.Close();
        }

        public void GoSurfaceEditor()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-surface-editor");
            _flyoutService.Close();
        }

        public void ShowDebugger()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-debugger");
            _flyoutService.Close();
        }

        public void GoSettings()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-settings");
            _flyoutService.Close();
        }

        public void Run()
        {
            Process.Start(ArtemisPath, ArtemisParams);
            _flyoutService.Close();
        }

        public async void Restart()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/restart");
            _flyoutService.Close();
        }
    }
}
