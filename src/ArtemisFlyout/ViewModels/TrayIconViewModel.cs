using System;
using System.Diagnostics;
using ArtemisFlyout.Views;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Diagnostics;
using Avalonia.Platform;

namespace ArtemisFlyout.ViewModels
{
    public class TrayIconViewModel : ViewModelBase
    {
        public readonly TrayIcon trayIcon = new TrayIcon();

        public TrayIconViewModel()
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
      

            Program.MainWindowInstance = new MainWindow();
            MainWindow flyout = Program.MainWindowInstance;
            flyout.DataContext = new ArtemisViewModel();
            flyout.Show();
        }
    }
}
