using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;

namespace ArtemisFlyout.Screens
{
    public class FlyoutWindow<TViewModel> : Avalonia.ReactiveUI.ReactiveWindow<TViewModel> where TViewModel : class
    {
        private readonly int _screenHeight;
        private readonly int _screenWidth;

        public int AnimationDelay { get; set; } = 200;

        public int VerticalSpacing { get; set; } = 12;

        public FlyoutWindow()
        {
            _screenWidth = Screens.Primary.WorkingArea.Width;
            _screenHeight = Screens.Primary.WorkingArea.Height;
        }

        public async Task ShowAnimated()
        {
            
            this.PropertyChanged += FlyoutWindow_PropertyChanged;

            WindowStartupLocation = WindowStartupLocation.Manual;

            this.Find<Panel>("FlyoutPanelContainer").Width = Width;

            Show();

            Clock = Avalonia.Animation.Clock.GlobalClock;

            var showTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            showTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, (int)base.Width, HorizontalPosition);
            await Task.Delay(AnimationDelay);
        }

        public async Task CloseAnimated()
        {
            var closeTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseIn()
            };

            closeTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, HorizontalPosition, (int)base.Width);
            await Task.Delay(AnimationDelay);
            Close();
        }

        public void SetHeight(double newHeight)
        {
            var heightTransition = new DoubleTransition()
            {
                Property = FlyoutContainer.HeightProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            heightTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, Height, newHeight);
        }

        public void SetWidth(double newWidth)
        {
            var widthTransition = new DoubleTransition()
            {
                Property = FlyoutContainer.WidthProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            widthTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, base.Width, newWidth);
        }

        private void FlyoutWindow_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Height" || e.Property.Name == "Width" || e.Property.Name == "HorizontalPosition")
            {
                Position = new PixelPoint(_screenWidth + (-(int)base.Width), _screenHeight + (-VerticalSpacing) + (-(int)Height));
            }
        }

        // ReSharper disable once StaticMemberInGenericType
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
                base.Width = _maskedWidth + HorizontalSpacing;

            }
        }

        private int _horizontalSpacing = 12;
        public int HorizontalSpacing
        {
            get
            {
                return _horizontalSpacing;
            }
            set
            {

                _horizontalSpacing = value;
                base.Width = _maskedWidth + _horizontalSpacing;
            }
        }
    }
}