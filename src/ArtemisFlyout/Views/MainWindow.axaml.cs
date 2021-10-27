using System;
using ArtemisFlyout.Util;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia.Enums;
using RoutedEventArgs = Avalonia.Interactivity.RoutedEventArgs;
using Window = Avalonia.Controls.Window;

namespace ArtemisFlyout.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var primaryScreen = Screens.Primary.WorkingArea;

            WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.Manual;
            Position = new PixelPoint(primaryScreen.Width - 310, primaryScreen.Height - 420);
            Deactivated += MainWindow_Deactivated;
            Width = 280;
            Height = 415;
            //Height = 0;
            var t = new DoubleTransition()
            {

                Property = Window.HeightProperty,
                Duration = TimeSpan.FromMilliseconds(1000),
                Easing = new QuadraticEaseOut()
            };

            //t.Apply(this, Avalonia.Animation.Clock.GlobalClock, 0d, 1090d);
        }

        private void MainWindow_Deactivated(object? sender, System.EventArgs e)
        {
            Close();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BtnHome_OnClick(object? sender, RoutedEventArgs e)
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/bring-to-foreground");
        }

        private async void BtnRestart_OnClick(object? sender, RoutedEventArgs e)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/restart");
            Program.MainWindowInstance.Close();
        }

        private void BtnWorkshop_OnClick(object? sender, RoutedEventArgs e)
        {
      

        }
    }
}
