using System.Reactive.Disposables;
using ArtemisFlyout.Services.ArtemisServices;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly ArtemisDeviceTogglesViewModel _artemisDeviceTogglesViewModel;
        private readonly ArtemisMainControlViewModel _artemisMainControlViewModel;
        private int _activePageIndex;

        public FlyoutContainerViewModel(
            IArtemisService artemisService,
            ArtemisDeviceTogglesViewModel artemisDeviceTogglesViewModel,
            ArtemisMainControlViewModel artemisMainControlViewModel)
        {
            _artemisService = artemisService;
            _artemisMainControlViewModel = artemisMainControlViewModel;
            _artemisDeviceTogglesViewModel = artemisDeviceTogglesViewModel;


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
        }

        public ArtemisMainControlViewModel ArtemisMainControlViewModel => _artemisMainControlViewModel;
        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel => _artemisDeviceTogglesViewModel;

        public int ActivePageindex
        {
            get => _activePageIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _activePageIndex, value);
            }
        }


        public void SetActivePageIndex(int newPageIndex)
        {
            ActivePageindex = newPageIndex;
        }

        public void GoBack()
        {
            ActivePageindex = 0;
        }

        public void Restart()
        {
            _artemisService.RestartArtemis();
        }

        public void GoHome()
        {
            _artemisService.GoHome();
        }

        public void GoWorkshop()
        {
            _artemisService.GoWorkshop();
        }

        public void GoSurfaceEditor()
        {
            _artemisService.GoSurfaceEditor();
        }

        public void ShowDebugger()
        {
            _artemisService.ShowDebugger();
        }

        public void GoSettings()
        {
            _artemisService.GoSettings();
        }
    }
}
