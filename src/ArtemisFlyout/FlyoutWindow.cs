using System;
using System.Threading.Tasks;
using ArtemisFlyout.Views;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;

namespace ArtemisFlyout
{
    public class FlyoutWindow<TViewModel> : Avalonia.ReactiveUI.ReactiveWindow<TViewModel> where TViewModel : class
    {
        private readonly int _screenHeight;
        private readonly int _screenWidth;

        public int AnimationDelay { get; set; } = 200;
        public int HorizontalSpacing { get; set; } = 12;
        public int VerticalSpacing { get; set; } = 12;

        public FlyoutWindow()
        {
            _screenWidth = Screens.Primary.WorkingArea.Width;
            _screenHeight = Screens.Primary.WorkingArea.Height;
        }

        private bool _animating;

        public void ShowAnimated()
        {
            if (_animating)
                return;
            _animating = true;
            WindowStartupLocation = WindowStartupLocation.Manual;

            Position = new PixelPoint(_screenWidth + (-(int)base.Width), _screenHeight + (-VerticalSpacing) + (-(int)Height));

            this.Find<Panel>("FlyoutPanelContainer").Width = Width;


            Show();
            Clock = Avalonia.Animation.Clock.GlobalClock;

            var t = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            t.Apply(this, Avalonia.Animation.Clock.GlobalClock, (int)base.Width, 0);

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

            t.Apply(this, Avalonia.Animation.Clock.GlobalClock, 0, (int)base.Width);
            // -10 is enough to avoid windows flashing
            await Task.Delay(AnimationDelay - 10);
            Close();
            _animating = false;
        }

        public static readonly DirectProperty<FlyoutContainer, int> HorizontalPositionProperty =
            AvaloniaProperty.RegisterDirect<FlyoutContainer, int>(
                nameof(HorizontalPosition),
                o => o.HorizontalPosition,
                (o, v) => o.HorizontalPosition = v);

        private int _horizontalPosition;
        public int HorizontalPosition
        {
            get => _horizontalPosition;
            set
            {
                SetAndRaise(HorizontalPositionProperty, ref _horizontalPosition, value);
                RenderTransform = new TranslateTransform(value, 0);
            }
        }

        private double _maskedWidth;
        public new double Width
        {
            get
            {
                return _maskedWidth;
            }
            set
            {
                _maskedWidth = value;
                base.Width = value + HorizontalSpacing;
            }
        }
    }
}
