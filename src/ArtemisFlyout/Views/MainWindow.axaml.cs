    #nullable enable
using System;
using System.Threading.Tasks;
using ArtemisFlyout.Util;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia.Enums;
using RoutedEventArgs = Avalonia.Interactivity.RoutedEventArgs;
using Window = Avalonia.Controls.Window;

namespace ArtemisFlyout.Views
{
    public partial class MainWindow : Window
    {
        private const int AnimationDelay = 300;
        private const int FlyoutWidth = 290;
        private const int FlyoutHeight = 430;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var primaryScreen = Screens.Primary.WorkingArea;

            WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.Manual;
            //TODO: Use taskbar height and binded Width and Height
            Position = new PixelPoint(primaryScreen.Width - FlyoutWidth, primaryScreen.Height - FlyoutHeight);
            Deactivated += MainWindow_Deactivated;
        }

        private bool _animating;
        public void ShowAnimated()
        {
            if (_animating)
                return;
            _animating = true;

            Show();
            var filler = this.Find<Separator>("SepAnimationFiller");
            Width = FlyoutWidth;
            Height = FlyoutHeight;

            var t = new DoubleTransition()
            {

                Property = Separator.WidthProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new QuadraticEaseOut()
            };

            t.Apply(filler, Avalonia.Animation.Clock.GlobalClock, (double)FlyoutWidth, 0d);
            _animating = false;
        }

        public async void CloseAnimated()
        {
            if (_animating)
                return;
            _animating = true;

            var filler = this.Find<Separator>("SepAnimationFiller");
            var t = new DoubleTransition()
            {

                Property = Separator.WidthProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new QuadraticEaseIn()
            };

            t.Apply(filler, Avalonia.Animation.Clock.GlobalClock, 0d, (double)FlyoutWidth);
            await Task.Delay(AnimationDelay);
            Close();
            Program.MainWindowInstance = null;

            _animating = false;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            CloseAnimated();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private async void BtnRestart_OnClick(object? sender, RoutedEventArgs e)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/restart");
            CloseAnimated();
        }

        private async void BtnHome_OnClick(object? sender, RoutedEventArgs e)
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/bring-to-foreground");
        }

        private async void BtnWorkshop_OnClick(object? sender, RoutedEventArgs e)
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-workshop");
            CloseAnimated();
        }

        private async void BtnShowSurfaceEditor_OnClick(object? sender, RoutedEventArgs e)
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-surface-editor");
            CloseAnimated();
        }

        private async void BtnShowDebugger_OnClick(object? sender, RoutedEventArgs e)
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-debugger");
            CloseAnimated();
        }

        private async void BtnShowSettings_OnClick(object? sender, RoutedEventArgs e)
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-settings");
            CloseAnimated();
        }


    }
}
