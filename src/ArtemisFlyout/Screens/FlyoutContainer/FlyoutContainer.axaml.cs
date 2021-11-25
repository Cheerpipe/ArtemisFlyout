using System;
using System.Diagnostics;
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
        }

        private readonly int _screenHeight;
        private readonly int _screenWidth;

        public int ShowAnimationDelay { get; set; } = 250;
        public int CloseAnimationDelay { get; set; } = 200;
        public int ResizeAnimationDelay { get; set; } = 150;
        public int FlyoutSpacing { get; set; } = 12;


        public async Task ShowAnimated(bool isPreload = false)
        {
            PointerPressed += FlyoutPanelContainer_PointerPressed;
            PointerReleased += FlyoutPanelContainer_PointerReleased;
            PointerMoved += FlyoutPanelContainer_PointerMoved;
            PropertyChanged += FlyoutWindow_PropertyChanged;

            WindowStartupLocation = WindowStartupLocation.Manual;

            if (isPreload) WindowState = WindowState.Minimized;

            Position = new PixelPoint(_screenWidth - (int)(Width + 12), Position.Y);

            Show();

            Clock = Avalonia.Animation.Clock.GlobalClock;
            var showTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.VerticalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(ShowAnimationDelay),
                Easing = new CircularEaseOut()
            };

            if (!isPreload)
            {
                showTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, _screenHeight, GetTargetVerticalPosition());
                await Task.Delay(ShowAnimationDelay);
            }
        }

        #region Drag to move
        private async void FlyoutPanelContainer_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _isOnDrag = false;
            if (VerticalPosition >= GetTargetVerticalPosition() + GetTargetVerticalPosition() * 0.1f)
                await CloseAnimated(CloseAnimationDelay);
            else
                VerticalPosition = GetTargetVerticalPosition();
        }

        private double _previousPosition;
        private double _currentPosition;
        private void FlyoutPanelContainer_PointerMoved(object sender, PointerEventArgs e)
        {
            if (!_isOnDrag)
            {
                _previousPosition = e.GetPosition(this).Y;
                return;
            }

            if (e.Pointer.IsPrimary)
            {
                _currentPosition = this.PointToScreen(e.GetPosition(this)).Y;
                double delta = _previousPosition - _currentPosition;
                _previousPosition = _currentPosition;

                if (VerticalPosition <= GetTargetVerticalPosition() && delta > 0)
                {
                    VerticalPosition = GetTargetVerticalPosition();
                    return;
                }

                VerticalPosition -= (int)delta;
            }
        }

        public int GetTargetVerticalPosition() => _screenHeight - (int)(Height + FlyoutSpacing);

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
                    _previousPosition = this.PointToScreen(e.GetPosition(this)).Y;
                    _isOnDrag = true;
                    break;
            }
        }

        #endregion

        public async Task CloseAnimated(double animationDuration)
        {
            var closeTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.VerticalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(animationDuration),
                Easing = new CircularEaseIn(),
            };

            closeTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, VerticalPosition, _screenHeight);
            await Task.Delay(CloseAnimationDelay);
            Close();
        }

        public async Task CloseAnimated()
        {
            await CloseAnimated(CloseAnimationDelay);
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

        public static readonly AttachedProperty<int> HorizontalPositionProperty = AvaloniaProperty.RegisterAttached<FlyoutContainer, Control, int>(nameof(HorizontalPosition));

        public int HorizontalPosition
        {
            get => GetValue(HorizontalPositionProperty);
            set
            {
                SetValue(HorizontalPositionProperty, value);
                RenderTransform = new TranslateTransform(value, 0);
            }
        }

        public static readonly AttachedProperty<int> VerticalPositionProperty = AvaloniaProperty.RegisterAttached<FlyoutContainer, Control, int>(nameof(VerticalPosition));

        public int VerticalPosition
        {
            get => GetValue(VerticalPositionProperty);
            set
            {
                SetValue(VerticalPositionProperty, value);
                Position = Position.WithY(value);
            }
        }
        static FlyoutContainer()
        {
            HorizontalPositionProperty.Changed.Subscribe(HorizontalPositionChanged);
            VerticalPositionProperty.Changed.Subscribe(VerticalPositionChanged);
        }

        private void FlyoutWindow_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Width":
                    Position = Position.WithX(_screenWidth - ((int) Width + FlyoutSpacing));
                    break;
                case "Height":
                    Position = Position.WithY(_screenHeight - ((int)Height + FlyoutSpacing));
                    Debug.WriteLine(Height);
                    break;
            }
        }

        private static void HorizontalPositionChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var flyoutContainer = (FlyoutContainer)e.Sender;
            var newPositionValue = (int)e.NewValue!;
            flyoutContainer.HorizontalPosition = newPositionValue;
        }

        private static void VerticalPositionChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var flyoutContainer = (FlyoutContainer)e.Sender;
            var newPositionValue = (int)e.NewValue!;
            flyoutContainer.VerticalPosition = newPositionValue;
        }
    }
}
