using System;
using ArtemisFlyout.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using AvaloniaTrayIcon = Avalonia.Controls.TrayIcon;

namespace ArtemisFlyout.ViewModels
{
    public class TrayIcon
    {
        public readonly AvaloniaTrayIcon trayIcon = new AvaloniaTrayIcon();

        public TrayIcon()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var icon = new WindowIcon(assets.Open(new Uri(@"resm:ArtemisFlyout.Assets.bow.ico")));
            trayIcon.Icon = icon;
            trayIcon.Clicked += TrayIcon_Clicked;
            trayIcon.Menu = new NativeMenu();

            NativeMenuItem exitMenu = new NativeMenuItem("Exit Artemis Flyout");
            exitMenu.Click += ExitMenu_Click;
            trayIcon.Menu.Items.Add(exitMenu);
        }

        private void ExitMenu_Click(object? sender, EventArgs e)
        {
            Program.runCancellationTokenSource.Cancel();
        }

        private void TrayIcon_Clicked(object? sender, System.EventArgs e)
        {
            if (Program.MainWindowInstance == null)
            {
                Program.MainWindowInstance = new MainWindow();
                MainWindow flyout = Program.MainWindowInstance;
                flyout.DataContext = new ArtemisViewModel();
                flyout.ShowAnimated();
            }
        }
    }
}
