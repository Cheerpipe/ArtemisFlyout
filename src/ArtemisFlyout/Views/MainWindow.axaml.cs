using System;
using System.Reflection;
using System.Threading.Tasks;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace ArtemisFlyout.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private const int FlyoutHorizontalSpacing = 12;
        private const int FlyoutVerticalSpacing = 25;
        private const int AnimationDelay = 200;

        private int _flyoutWidth;
        private int _flyoutHeight;

        public MainWindow()
        {
            // If you put a WhenActivated block here, your activatable view model 
            // will also support activation, otherwise it won't.
           // ViewModel = viewModel;
            this.WhenActivated(disposables => { /* Handle interactions etc. */ });
            AvaloniaXamlLoader.Load(this);

#if DEBUG
            this.AttachDevTools();
#endif

        }

        private bool _animating;
        public void ShowAnimated()
        {
            if (_animating)
                return;
            _animating = true;

            var primaryScreen = Screens.Primary.WorkingArea;

            WindowStartupLocation = WindowStartupLocation.Manual;

            _flyoutWidth = (int)this.Find<Panel>("FlyoutPanelContainer").Width + FlyoutHorizontalSpacing;
            _flyoutHeight = (int)this.Find<Panel>("FlyoutPanelContainer").Height + FlyoutVerticalSpacing;

            Position = new PixelPoint(primaryScreen.Width - _flyoutWidth, primaryScreen.Height - _flyoutHeight);
            Deactivated += MainWindow_Deactivated;


            Show();
            var filler = this.Find<Separator>("SepAnimationFiller");
            Width = _flyoutWidth;
            Height = _flyoutHeight;

            var t = new DoubleTransition()
            {

                Property = Separator.WidthProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            t.Apply(filler, Avalonia.Animation.Clock.GlobalClock, (double)_flyoutWidth, 0d);

            _animating = false;
        }

        public async void CloseAnimated()
        {
            if (_animating)
                return;
            _animating = true;

            Deactivated -= MainWindow_Deactivated;

            var filler = this.Find<Separator>("SepAnimationFiller");
            var t = new DoubleTransition()
            {

                Property = Separator.WidthProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseIn()
            };

            t.Apply(filler, Avalonia.Animation.Clock.GlobalClock, 0d, (double)_flyoutWidth);

            // -10 is enough to avoid windows flashing
            await Task.Delay(AnimationDelay - 10);
            Close();
            _animating = false;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            CloseAnimated();
        }

    }
}
