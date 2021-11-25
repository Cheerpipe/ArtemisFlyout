using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Pages;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.Styling;
using ReactiveUI;

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly IFlyoutService _flyoutService;
        private readonly List<LedColorPickerLed> _ledColorPickerLeds;
        private readonly Random _random = new();
        private readonly byte _colorMaskOpacity;
        private Timer _backgroundBrushRefreshTimer;
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
                        _backgroundBrushRefreshTimer?.DisposeAsync();
                        _backgroundBrushRefreshTimer = null;
                    })
                    .DisposeWith(disposables);
            });

            FlyoutWindowWidth = MainPageWidth;
            FlyoutWindowHeight = MainPageHeight;
            _ledColorPickerLeds = configurationService.Get().LedColorPickerLedSettings.LedColorPickerLeds;
            _colorMaskOpacity = Math.Clamp(configurationService.Get().LedColorPickerLedSettings.ColorMaskOpacity, byte.MinValue, byte.MaxValue);

            if (configurationService.Get().LedColorPickerLedSettings.KeepColorInSync)
            {
                _backgroundBrushRefreshTimer = new Timer(_ => UpdateColorBrush(), null, 0, 3000);
            }
            _artemisService.JsonDataModelValueSent += _artemisService_JsonDataModelValueSent;
            BackgroundBrush = CreateBackgroundBrush(GetBackgroundBrushColor());
        }

        private void UpdateColorBrush()
        {
            Color color = GetBackgroundBrushColor();
            Dispatcher.UIThread.Post(() => { BackgroundBrush = CreateBackgroundBrush(color); });
        }

        private void _artemisService_JsonDataModelValueSent(object sender, Events.JsonDataModelValueSentArgs e)
        {
            _backgroundBrushRefreshTimer?.Change(100, 3000);
        }

        private LinearGradientBrush CreateBackgroundBrush(Color color)
        {
            LinearGradientBrush brush = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
                EndPoint = new RelativePoint(0, 0, RelativeUnit.Relative)
            };
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(_colorMaskOpacity, color.R, color.G, color.B), 0d));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1d));
            return brush;
        }

        private Color GetBackgroundBrushColor()
        {

            return _artemisService.GetLedColor(
                _ledColorPickerLeds[_random.Next(_ledColorPickerLeds.Count - 1)].DeviceType,
                _ledColorPickerLeds[_random.Next(_ledColorPickerLeds.Count - 1)].LedId);
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
                FlyoutWidth = _flyoutWindowWidth ;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                //Workaround
                var thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
                return thm.RequestedTheme == "Light" ? Colors.White : Colors.Black;
            }
        }

        public Color InvertedBackgroundColor => Color.FromUInt32(BackgroundColor.ToUint32() ^ 0xFFFFFFFF);

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
                FlyoutHeight = _flyoutWindowHeight;
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
