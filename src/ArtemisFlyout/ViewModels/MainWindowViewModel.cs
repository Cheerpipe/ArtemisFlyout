using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reflection;
using ArtemisFlyout.Services;
using ArtemisFlyout.Util;
using MessageBox.Avalonia.Enums;
using Ninject;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel { get; private set; }
        public ArtemisMainControlViewModel ArtemisMainControlViewModel { get; } = new();
        private int _activePageIndex;

        public MainWindowViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;

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

        public bool IsArtemisRunning => Process.GetProcessesByName("Artemis.UI").Length > 0;

        public int ActivePageindex
        {
            get => _activePageIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _activePageIndex, value);
            }
        }

        public void GoBack()
        {
            ActivePageindex = 0;
        }

        public void SetActivePageIndex(int pageIndex)
        {
            ActivePageindex = pageIndex;
        }

        private async void Restart()
        {
            _artemisService.Restart();
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
