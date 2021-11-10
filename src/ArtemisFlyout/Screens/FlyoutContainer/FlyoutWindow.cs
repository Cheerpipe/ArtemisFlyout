using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace ArtemisFlyout.Screens
{
    public class FlyoutWindow<TViewModel> : Avalonia.ReactiveUI.ReactiveWindow<TViewModel> where TViewModel : class
    {
        private readonly int _screenHeight;
        private readonly int _screenWidth;

        public int RevealAnimationDelay { get; set; } = 250;
        public int ResizeAnimationDelay { get; set; } = 250;

        public int VerticalSpacing { get; set; } = 12;
        Panel FlyoutPanelContainer;

        public FlyoutWindow()
        {
            _screenWidth = Screens.Primary.WorkingArea.Width;
            _screenHeight = Screens.Primary.WorkingArea.Height;
        }

        public async Task ShowAnimated()
        {


            FlyoutPanelContainer = this.Find<Panel>("FlyoutPanelContainer");
            FlyoutPanelContainer.PointerPressed += FlyoutPanelContainer_PointerPressed;
            FlyoutPanelContainer.PointerReleased += FlyoutPanelContainer_PointerReleased;
            FlyoutPanelContainer.PointerMoved += FlyoutPanelContainer_PointerMoved;

            PropertyChanged += FlyoutWindow_PropertyChanged;

            WindowStartupLocation = WindowStartupLocation.Manual;

            Show();

            Clock = Avalonia.Animation.Clock.GlobalClock;

            var showTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(RevealAnimationDelay),
                Easing = new CircularEaseOut()
            };

            showTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, (int)Width, HorizontalPosition);
            await Task.Delay(RevealAnimationDelay);
        }

        #region Drag to move
        private async void FlyoutPanelContainer_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            isOnDrag = false;
            if (HorizontalPosition >= this.Width / 2)
            {
                await CloseAnimated(RevealAnimationDelay * 0.3d);
            }
            else
            {
                HorizontalPosition = 0;
            }
        }
        double previousPosition = 0;
        private void FlyoutPanelContainer_PointerMoved(object sender, PointerEventArgs e)
        {
            if (!isOnDrag)
            {
                previousPosition = e.Device.GetPosition(this).X;
                return;
            }
            if (e.Pointer.IsPrimary)
            {
                double currentPosition = e.Device.GetPosition(this).X;
                double delta = previousPosition - currentPosition;
                previousPosition = currentPosition;


                Debug.WriteLine(currentPosition);

                if (currentPosition < 0)
                    return;

                if (HorizontalPosition <= 0 && delta > 0)
                    return;
                HorizontalPosition = HorizontalPosition - (int)delta;
            }
        }

        bool isOnDrag = false;
        private void FlyoutPanelContainer_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.Pointer.IsPrimary)
            {
                isOnDrag = true;
            }
        }

        #endregion

        public async Task CloseAnimated(double animationDuration)
        {
            var closeTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.HorizontalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(animationDuration),
                Easing = new CircularEaseIn(),
            };

            closeTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, HorizontalPosition, (int)Width);
            await Task.Delay(RevealAnimationDelay);
            Close();
        }

        public async Task CloseAnimated()
        {
            await CloseAnimated(RevealAnimationDelay);
        }

        public void SetHeight(double newHeight)
        {
            var heightTransition = new DoubleTransition()
            {
                Property = HeightProperty,
                Duration = TimeSpan.FromMilliseconds(ResizeAnimationDelay),
                Easing = new CircularEaseOut()
            };

            heightTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, Height, newHeight);
        }

        public void SetWidth(double newWidth)
        {
            var widthTransition = new DoubleTransition()
            {
                Property = WidthProperty,
                Duration = TimeSpan.FromMilliseconds(ResizeAnimationDelay),
                Easing = new CircularEaseOut()
            };

            widthTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, Width, newWidth);
        }

        private void FlyoutWindow_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Height" || e.Property.Name == "Width" || e.Property.Name == "HorizontalPosition")
            {
                Position = new PixelPoint(_screenWidth + (-(int)Width), _screenHeight + (-VerticalSpacing) + (-(int)Height));
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
    }
}