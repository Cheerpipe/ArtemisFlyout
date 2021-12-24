using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Avalonia.ReactiveUI;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainer : ReactiveWindow<FlyoutContainerViewModel>
    {
        [Flags]
        public enum SetWindowPosFlags : uint
        {
            SWP_ASYNCWINDOWPOS = 0x4000,
            SWP_DEFERERASE = 0x2000,
            SWP_DRAWFRAME = 0x0020,
            SWP_FRAMECHANGED = 0x0020,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOACTIVATE = 0x0010,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOMOVE = 0x0002,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOREDRAW = 0x0008,
            SWP_NOREPOSITION = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
            SWP_NOSIZE = 0x0001,
            SWP_NOZORDER = 0x0004,
            SWP_SHOWWINDOW = 0x0040,

            SWP_RESIZE = SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOZORDER
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

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
            _containerBorder = this.Find<Border>("ContainerBorder");
        }

        private readonly int _screenHeight;
        private readonly int _screenWidth;
        private readonly Border _containerBorder;

        public int ShowAnimationDelay { get; set; } = 250;
        public int ContentRevealAnimationDelay { get; set; } = 1000;
        public int CloseAnimationDelay { get; set; } = 250;
        public int ResizeAnimationDelay { get; set; } = 150;
        public int FlyoutSpacing { get; set; } = 12;

        public async Task ShowAnimated()
        {
            PointerPressed += FlyoutPanelContainer_PointerPressed;
            PointerReleased += FlyoutPanelContainer_PointerReleased;
            PointerMoved += FlyoutPanelContainer_PointerMoved;
            PropertyChanged += FlyoutWindow_PropertyChanged;

            WindowStartupLocation = WindowStartupLocation.Manual;

            Position = new PixelPoint(_screenWidth - (int)(Width + FlyoutSpacing), Position.Y);

            Show();
            Activate();

            Clock = Avalonia.Animation.Clock.GlobalClock;
            IntegerTransition showTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.VerticalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(ShowAnimationDelay),
                Easing = new ExponentialEaseOut()
            };

            showTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, _screenHeight, GetTargetVerticalPosition());

            Panel mainContainerPanel = this.Find<Panel>("MainContainerPanel");
            TransformOperationsTransition marginTransition = new TransformOperationsTransition()
            {
                Property = FlyoutContainer.RenderTransformProperty,
                Duration = TimeSpan.FromMilliseconds(ContentRevealAnimationDelay),
                Easing = new ExponentialEaseOut()
            };
            marginTransition.Apply(mainContainerPanel, Avalonia.Animation.Clock.GlobalClock, TransformOperations.Parse("translate(-20px, 0px)"), TransformOperations.Parse("translate(0px, 0px)"));

            await Task.Delay(ShowAnimationDelay);

            //Workaround to activate animation after flyout is showed because duration property can't be binded
            if (_containerBorder is not null)
            {
                BrushTransition backgroundTransition = new BrushTransition()
                {
                    Property = Border.BackgroundProperty,
                    Duration = TimeSpan.FromMilliseconds(500)
                };

                _containerBorder.Transitions = new Transitions();
                _containerBorder.Transitions.Add(backgroundTransition);
            }
            Activate();
        }

        public async Task CloseAnimated(double animationDuration)
        {
            IntegerTransition closeTransition = new IntegerTransition()
            {
                Property = FlyoutContainer.VerticalPositionProperty,
                Duration = TimeSpan.FromMilliseconds(animationDuration),
                Easing = new ExponentialEaseIn(),
            };

            closeTransition.Apply(this, Avalonia.Animation.Clock.GlobalClock, VerticalPosition, _screenHeight);
            await Task.Delay(CloseAnimationDelay);
            Close();
        }

        public async Task CloseAnimated()
        {
            await CloseAnimated(CloseAnimationDelay);
        }


        #region Drag to move
        private async void FlyoutPanelContainer_PointerReleased(object sender, PointerReleasedEventArgs e)
        {

            _isOnDrag = false;
            if (VerticalPosition >= GetTargetVerticalPosition() * 1.1f)
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
                _previousPosition = this.PointToScreen(e.GetPosition(this)).Y;
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
                    Position = Position.WithX(_screenWidth - ((int)Width + FlyoutSpacing));
                    _isOnDrag = false;
                    break;
                case "Height":
                    VerticalPosition = _screenHeight - ((int)Height + FlyoutSpacing);
                    _isOnDrag = false;
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
