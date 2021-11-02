using System;
using ArtemisFlyout.Services.FlyoutServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using AvaloniaTrayIcon = Avalonia.Controls.TrayIcon;

namespace ArtemisFlyout.Services.TrayIcon
{
    public class TrayIconService : ITrayIconService
    {
        private readonly IFlyoutService _flyoutService;
        private readonly AvaloniaTrayIcon _trayIcon = new AvaloniaTrayIcon();

        public TrayIconService(IFlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var icon = new WindowIcon(assets.Open(new Uri(@"resm:ArtemisFlyout.Assets.bow.ico")));
            _trayIcon.Icon = icon;
            _trayIcon.Clicked += TrayIcon_Clicked;
        }

        public void Show()
        {
            _trayIcon.Menu = new NativeMenu();
            NativeMenuItem exitMenu = new NativeMenuItem("Exit Artemis Flyout");
            exitMenu.Click += ExitMenu_Click;
            _trayIcon.Menu.Items.Add(exitMenu);
            _trayIcon.IsVisible = true;
        }
        public void Hide()
        {
            _trayIcon.IsVisible = false;
        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            //TODO: DI
            Program.runCancellationTokenSource.Cancel();
        }

        private void TrayIcon_Clicked(object sender, EventArgs e)
        {
            _flyoutService.Show();
        }
    }
}
