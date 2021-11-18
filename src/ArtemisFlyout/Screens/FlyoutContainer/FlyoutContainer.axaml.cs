using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainer : ReactiveWindow<FlyoutContainerViewModel>
    {
        public FlyoutContainer()
        {
            this.WhenActivated(disposables =>
            {
                /* Handle interactions etc. */
            });
            AvaloniaXamlLoader.Load(this);

#if DEBUG
            this.AttachDevTools();
#endif

            _screenWidth = Screens.Primary.WorkingArea.Width;
            _screenHeight = Screens.Primary.WorkingArea.Height;

            RevealAnimationDelay = 250;
            ResizeAnimationDelay = 150;
        }

        private readonly int _screenHeight;
        private readonly int _screenWidth;

        public int RevealAnimationDelay { get; set; }
        public int ResizeAnimationDelay { get; set; }

        Panel _flyoutPanelContainer;

        public async Task ShowAnimated(bool isPreload = false)
        {


            _flyoutPanelContainer = this.Find<Panel>("FlyoutPanelContainer");
            _flyoutPanelContainer.PointerPressed += FlyoutPanelContainer_PointerPressed;
            _flyoutPanelContainer.PointerReleased += FlyoutPanelContainer_PointerReleased;
            _flyoutPanelContainer.PointerMoved += FlyoutPanelContainer_PointerMoved;

            PropertyChanged += FlyoutWindow_PropertyChanged;

            WindowStartupLocation = WindowStartupLocation.Manual;

            if (isPreload)
            {
                this.WindowState = WindowState.Minimized;
                HorizontalPosition = Screens.All.Sum(s => s.WorkingArea.Width);
            }

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
            _isOnDrag = false;

            if (HorizontalPosition >= (this.Width + ViewModel!.FlyoutSpacing) / 2)
                await CloseAnimated(RevealAnimationDelay * 0.25d);
            else
                HorizontalPosition = 0;
        }

        private double _previousPosition;
        private double _currentPosition;
        private void FlyoutPanelContainer_PointerMoved(object sender, PointerEventArgs e)
        {
            if (!_isOnDrag)
            {
                _previousPosition = e.GetPosition(this).X;
                return;
            }

            if (e.Pointer.IsPrimary)
            {
                _currentPosition = e.GetPosition(this).X;
                double delta = _previousPosition - _currentPosition;
                _previousPosition = _currentPosition;

                if ((_currentPosition < 0) || (HorizontalPosition <= 0 && delta > 0))
                    return;
                HorizontalPosition = HorizontalPosition - (int)delta;
            }

        }

        private bool _isOnDrag;
        private void FlyoutPanelContainer_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (!e.Pointer.IsPrimary) return;

            switch (e.Source)
            {
                case Border border when (border.TemplatedParent is ComboBox) || (border.TemplatedParent is ComboBoxItem):
                case TextBlock:
                    return;
                default:
                    _previousPosition = e.GetPosition(this).X;
                    _isOnDrag = true;
                    break;
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
                Position = new PixelPoint(_screenWidth + (-(int)Width), _screenHeight + (-(int)Height));
            }
        }

        public static readonly AttachedProperty<int> HorizontalPositionProperty = AvaloniaProperty.RegisterAttached<FlyoutContainer, Control, int>("HorizontalPosition");

        public int HorizontalPosition
        {
            get => GetValue(HorizontalPositionProperty);
            set
            {
                SetValue(HorizontalPositionProperty, value);
                RenderTransform = new TranslateTransform(value, 0);
            }
        }
        static FlyoutContainer()
        {
            HorizontalPositionProperty.Changed.Subscribe(IsOpenChanged);
        }

        private static void IsOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var flyoutContainer = (FlyoutContainer)e.Sender;
            var newHorizontalPositionValue = (int)e.NewValue!;
            flyoutContainer.HorizontalPosition = newHorizontalPositionValue;
        }
    }
}
