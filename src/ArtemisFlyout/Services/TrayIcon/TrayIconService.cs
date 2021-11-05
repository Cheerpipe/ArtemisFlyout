using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using AvaloniaTrayIcon = Avalonia.Controls.TrayIcon;

namespace ArtemisFlyout.Services
{
    public class TrayIconService : ITrayIconService
    {
        private readonly IFlyoutService _flyoutService;
        private readonly AvaloniaTrayIcon _trayIcon;

        public TrayIconService(IFlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
            _flyoutService.Preload();
            _trayIcon = new AvaloniaTrayIcon();
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var icon = new WindowIcon(assets.Open(new Uri(@"resm:ArtemisFlyout.Assets.bow.ico")));
            _trayIcon.Icon = icon;
            _trayIcon.Clicked += TrayIcon_Clicked;
        }

        public void Show()
        {
            _trayIcon.Menu = new NativeMenu();
            NativeMenuItem exitMenu = new ("Exit Artemis Flyout");
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
            Program.RunCancellationTokenSource.Cancel();
        }

        private void TrayIcon_Clicked(object sender, EventArgs e)
        {
            _flyoutService.Toggle();
        }
    }
}
