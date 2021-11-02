using System;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Services.TrayIcon;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Ninject;
using AvaloniaTrayIcon = Avalonia.Controls.TrayIcon;

namespace ArtemisFlyout.Services
{
    public class TrayIconService : ITrayIconService
    {
        private readonly IFlyoutService _flyoutService;
        private readonly AvaloniaTrayIcon trayIcon = new AvaloniaTrayIcon();

        public TrayIconService(IFlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var icon = new WindowIcon(assets.Open(new Uri(@"resm:ArtemisFlyout.Assets.bow.ico")));
            trayIcon.Icon = icon;
            trayIcon.Clicked += TrayIcon_Clicked;
        }

        public void Show()
        {
            trayIcon.Menu = new NativeMenu();
            NativeMenuItem exitMenu = new NativeMenuItem("Exit Artemis Flyout");
            exitMenu.Click += ExitMenu_Click;
            trayIcon.Menu.Items.Add(exitMenu);
            trayIcon.IsVisible = true;
        }
        public void Hide()
        {
            trayIcon.IsVisible = false;
        }

        private void ExitMenu_Click(object? sender, EventArgs e)
        {
            //TODO: DI
            Program.runCancellationTokenSource.Cancel();
        }

        private void TrayIcon_Clicked(object? sender, System.EventArgs e)
        {
            _flyoutService.Show();
        }
    }
}
