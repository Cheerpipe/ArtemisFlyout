using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ArtemisFlyout.Views;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;

namespace ArtemisFlyout
{
    public class FlyoutWindow<TViewModel> : Avalonia.ReactiveUI.ReactiveWindow<TViewModel> where TViewModel : class
    {
        private readonly int _screenHeight;
        private readonly int _screenWidth;
        private bool _showing;

        public int AnimationDelay { get; set; } = 250;

        public int VerticalSpacing { get; set; } = 12;

        public FlyoutWindow()
        {
            _screenWidth = Screens.Primary.WorkingArea.Width;
            _screenHeight = Screens.Primary.WorkingArea.Height;

        }

        public void ShowAnimated()
        {
            PropertyChanged += FlyoutWindow_PropertyChanged;
            MaxWidth = Width;
            ClipToBounds = true;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Position = new PixelPoint(_screenWidth, (int)(_screenHeight + (-Height) + (-HorizontalSpacing)));
            this.Find<Panel>("FlyoutPanelContainer").Width = Width;

            var showTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseOut()
            };

            Show();
            _showing = true;
            showTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, _screenWidth, (int)(_screenWidth + (-Width) + (-HorizontalSpacing)));
        }

        public async Task CloseAnimated()
        {
            var closeTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(AnimationDelay),
                Easing = new CircularEaseIn()
            };

            _showing = false;
            closeTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, Position.X, _screenWidth);
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

            widthTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, Width, newWidth);
        }

        private void FlyoutWindow_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Height" /*|| e.Property.Name == "Width" || e.Property.Name == "HorizontalPosition"*/)
            {
                Position = new PixelPoint(Position.X, _screenHeight + (-VerticalSpacing) + (-(int)Height));
            }
            else if (e.Property.Name == "HorizontalPosition")
            {
                lock (this)
                {
                    int calculatedWidth = _screenWidth + (-HorizontalPosition);
                    BeginBatchUpdate();
                    Opacity = (calculatedWidth > 48) ? 1 : 0;
                    TransparencyLevelHint =
                        (calculatedWidth > 48)
                            ? WindowTransparencyLevel.AcrylicBlur
                            : WindowTransparencyLevel.Transparent;

                    Width = calculatedWidth;
                    Position = new PixelPoint(HorizontalPosition, Position.Y);
                    EndBatchUpdate();
                    InvalidateArrange();
                    InvalidateVisual();
                }
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
            }
        }

        public int HorizontalSpacing { get; set; } = 12;
    }
}
