using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Timers;
using ArtemisFlyout.Pages;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly IFlyoutService _flyoutService;
        private readonly string _colorPickerDeviceTypeName;
        private readonly string _colorPickerLedIdName;
        private readonly Timer _backgroundBrushRefreshTimer;
        private int _activePageIndex;
        private const int MainPageHeight = 530;
        private const int MainPageWidth = 290;

        public FlyoutContainerViewModel(
            IArtemisService artemisService,
            IConfigurationService configurationService,
            IFlyoutService flyoutService,
            ArtemisDeviceTogglesViewModel artemisDeviceTogglesViewModel,
            ArtemisLightControlViewModel artemisMainControlViewModel,
            ArtemisCustomProfileViewModel artemisCustomProfileViewModel)
        {
            _artemisService = artemisService;
            ArtemisLightControlViewModel = artemisMainControlViewModel;
            ArtemisDeviceTogglesViewModel = artemisDeviceTogglesViewModel;
            ArtemisCustomProfileViewModel = artemisCustomProfileViewModel;
            _flyoutService = flyoutService;

            this.WhenActivated(disposables =>
            {
                /* Handle activation */
                Disposable
                    .Create(() =>
                    {
                        /* Handle deactivation */
                        _backgroundBrushRefreshTimer?.Stop();
                        _backgroundBrushRefreshTimer?.Dispose();
                    })
                    .DisposeWith(disposables);
            });

            FlyoutWindowWidth = MainPageWidth;
            FlyoutWindowHeight = MainPageHeight;
            _colorPickerDeviceTypeName = configurationService.Get().KeyColorPicker.DeviceType;
            _colorPickerLedIdName = configurationService.Get().KeyColorPicker.LedId;
            if (configurationService.Get().KeyColorPicker.KeepColorInSync)
            {
                _backgroundBrushRefreshTimer = new Timer();
                _backgroundBrushRefreshTimer.Interval = 1000;
                _backgroundBrushRefreshTimer.Elapsed += _backgroundBrushRefreshTimer_Elapsed;
                _backgroundBrushRefreshTimer.Start();
            }

            _artemisService.JsonDataModelValueSent += _artemisService_JsonDataModelValueSent;
            BackgroundBrush = CreateBackgroundBrush(GetBackgroundBrushColor());
        }

        private void _backgroundBrushRefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Color color = GetBackgroundBrushColor();
            Dispatcher.UIThread.Post(() => { BackgroundBrush = CreateBackgroundBrush(color); });
        }

        private async void _artemisService_JsonDataModelValueSent(object sender, Events.JsonDataModelValueSentArgs e)
        {
            await Task.Delay(30);
            BackgroundBrush = CreateBackgroundBrush(GetBackgroundBrushColor());
        }

        private LinearGradientBrush CreateBackgroundBrush(Color color)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new RelativePoint(0, 1, RelativeUnit.Relative);
            brush.EndPoint = new RelativePoint(0, 0, RelativeUnit.Relative);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(50, color.R, color.G, color.B), 0d));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1d));
            return brush;
        }

        private Color GetBackgroundBrushColor()
        {
            return _artemisService.GetLedColor(_colorPickerDeviceTypeName, _colorPickerLedIdName);
        }

        public ArtemisLightControlViewModel ArtemisLightControlViewModel { get; }

        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel { get; }

        public ArtemisCustomProfileViewModel ArtemisCustomProfileViewModel { get; }

        public int ActivePageindex
        {
            get => _activePageIndex;
            set => this.RaiseAndSetIfChanged(ref _activePageIndex, value);
        }

        private double _flyoutWindowWidth;
        public double FlyoutWindowWidth
        {
            get => _flyoutWindowWidth;
            set
            {
                _flyoutService.SetWidth(value);
                _flyoutWindowWidth = value;
                FlyoutWidth = _flyoutWindowWidth - FlyoutSpacing;
            }
        }


        private LinearGradientBrush _backgroundBrush;
        public LinearGradientBrush BackgroundBrush
        {
            get => _backgroundBrush;
            set
            {
                _backgroundBrush = value;
                this.RaisePropertyChanged(nameof(BackgroundBrush));
            }
        }



        private double _flyoutWidth;
        public double FlyoutWidth
        {
            get => _flyoutWidth;
            set => this.RaiseAndSetIfChanged(ref _flyoutWidth, value);
        }


        private double _flyoutWindowHeight;
        public double FlyoutWindowHeight
        {
            get => _flyoutWindowHeight;
            set
            {
                _flyoutService.SetHeight(value);
                _flyoutWindowHeight = value;
                FlyoutHeight = _flyoutWindowHeight - FlyoutSpacing;
            }
        }

        private double _flyoutHeight;
        public double FlyoutHeight
        {
            get => _flyoutHeight;
            set => this.RaiseAndSetIfChanged(ref _flyoutHeight, value);
        }

        public bool CommandBarVisibleState => true;

        public void SetActivePageIndex(int newPageIndex)
        {
            ActivePageindex = newPageIndex;
        }

        public int FlyoutSpacing => 12;

        public void GoMainPage()
        {
            SetActivePageIndex(0);
            FlyoutWindowHeight = MainPageHeight;
            FlyoutWindowWidth = 290;
        }

        public void GoLaunchArtemisPage()
        {
            SetActivePageIndex(3);
            FlyoutWindowHeight = MainPageHeight;
            FlyoutWindowWidth = 290;
        }

        public void GoCustomProfile()
        {
            SetActivePageIndex(2);
            FlyoutWindowHeight = ArtemisCustomProfileViewModel.CalculatedHeight;
            FlyoutWindowWidth = 290;
        }

        public void GoDevicesPage()
        {
            SetActivePageIndex(1);
            FlyoutWindowHeight = ArtemisDeviceTogglesViewModel.CalculatedHeight;
            FlyoutWindowWidth = 330;
        }

        public void Restart()
        {
            _artemisService.RestartArtemis();
            _flyoutService.CloseAndRelease();
        }

        public void GoHome()
        {
            _artemisService.GoHome();
            _flyoutService.CloseAndRelease();
        }

        public void GoWorkshop()
        {
            _artemisService.GoWorkshop();
            _flyoutService.CloseAndRelease();
        }

        public void GoSurfaceEditor()
        {
            _artemisService.GoSurfaceEditor();
            _flyoutService.CloseAndRelease();
        }

        public void ShowDebugger()
        {
            _artemisService.ShowDebugger();
            _flyoutService.CloseAndRelease();
        }

        public void GoSettings()
        {
            _artemisService.GoSettings();
            _flyoutService.CloseAndRelease();
        }
    }
}
