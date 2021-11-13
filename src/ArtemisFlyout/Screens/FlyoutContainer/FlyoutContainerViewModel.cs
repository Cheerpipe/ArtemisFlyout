using System.Reactive.Disposables;
using ArtemisFlyout.Pages;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;
using Tmds.DBus;

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly IFlyoutService _flyoutService;
        private int _activePageIndex;
        private const int MainPageHeight = 530;
        private const int MainPageWidth = 290;

        public FlyoutContainerViewModel(
            IArtemisService artemisService,
            ArtemisDeviceTogglesViewModel artemisDeviceTogglesViewModel,
            ArtemisLightControlViewModel artemisMainControlViewModel,
            ArtemisCustomProfileViewModel artemisCustomProfileViewModel,
            IFlyoutService flyoutService)
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

                    })
                    .DisposeWith(disposables);
            });

            FlyoutWindowWidth = MainPageWidth;
            FlyoutWindowHeight = MainPageHeight;
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

        private double _flyoutWidth;
        public double FlyoutWidth
        {
            get => _flyoutWidth;
            set
            {
                this.RaiseAndSetIfChanged(ref _flyoutWidth, value);
            }
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
            set
            {
                this.RaiseAndSetIfChanged(ref _flyoutHeight, value);
            }
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
            FlyoutWindowHeight = (65 * ArtemisCustomProfileViewModel.Colors.Count) + 130;
            FlyoutWindowWidth = 290;
        }

        public void GoDevicesPage()
        {
            SetActivePageIndex(1);
            FlyoutWindowHeight = (46 * ArtemisDeviceTogglesViewModel.DeviceStates.Count) + 110;
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
