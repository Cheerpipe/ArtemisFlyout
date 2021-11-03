using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TypeSupport.Extensions;

// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Views
{
    public class FlyoutContainer : ReactiveWindow<FlyoutContainerViewModel>
    {
        private const int FlyoutHorizontalSpacing = 12;
        private const int FlyoutVerticalSpacing = 12;

        private const int AnimationDelay = 200;

        private int _flyoutWidth;
        private int _flyoutHeight;
        private int _screenHeight;

        public FlyoutContainer()
        {
            Deactivated += MainWindow_Deactivated;
            // If you put a WhenActivated block here, your activatable view model 
            // will also support activation, otherwise it won't.
            // ViewModel = viewModel;
            _screenWidth = Screens.Primary.WorkingArea.Width;
            _screenHeight = Screens.Primary.WorkingArea.Height;
            this.WhenActivated(disposables =>
            {
                /* Handle interactions etc. */
            });
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
            WindowStartupLocation = WindowStartupLocation.Manual;

            _flyoutWidth = (int)this.Find<Panel>("FlyoutPanelContainer").Width;
            _flyoutHeight = (int)this.Find<Panel>("FlyoutPanelContainer").Height;

            Position = new PixelPoint(_screenWidth + (-_flyoutWidth) + (-FlyoutHorizontalSpacing), _screenHeight + (-FlyoutVerticalSpacing) + (-_flyoutHeight));
            Width = _flyoutWidth+ FlyoutHorizontalSpacing;
            Height = _flyoutHeight;
            Show();
            Clock = Avalonia.Animation.Clock.GlobalClock;

            var t = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            t.Apply(this, Avalonia.Animation.Clock.GlobalClock, _flyoutWidth, 0);

            _animating = false;
        }

        public async void CloseAnimated()
        {
            if (_animating)
                return;
            _animating = true;

            var t = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseIn()
            };

            t.Apply(this, Avalonia.Animation.Clock.GlobalClock, 0, _flyoutWidth);
            // -10 is enough to avoid windows flashing
            await Task.Delay(AnimationDelay - 10);
            Close();
            _animating = false;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            CloseAnimated();
        }

        public static readonly DirectProperty<FlyoutContainer, int> HorizontalPositionProperty =
            AvaloniaProperty.RegisterDirect<FlyoutContainer, int>(
                nameof(HorizontalPosition),
                o => o.HorizontalPosition,
                (o, v) => o.HorizontalPosition = v);



        private int _horizontalPosition;
        private int _screenWidth;

        public int HorizontalPosition
        {
            get { return _horizontalPosition; }
            set
            {
                SetAndRaise(HorizontalPositionProperty, ref _horizontalPosition, value);
                RenderTransform = new TranslateTransform(value, 0);
            }
        }
    }
}