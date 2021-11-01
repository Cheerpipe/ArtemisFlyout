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
        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel { get; private set; }
        public ArtemisMainControlViewModel ArtemisMainControlViewModel { get; } = new();
        private int _activePageIndex;

        public MainWindowViewModel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var trayIconService = kernel.Get<ArtemisDeviceTogglesViewModel>();


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
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/restart");
            Program.MainWindowInstance.CloseAnimated();
        }

        public void GoHome()
        {
            ArtemisService.GoHome();
        }

        public void GoWorkshop()
        {
            ArtemisService.GoWorkshop();
        }

        public void GoSurfaceEditor()
        {
            ArtemisService.GoSurfaceEditor();
        }

        public void ShowDebugger()
        {
            ArtemisService.ShowDebugger();
        }

        public void GoSettings()
        {
            ArtemisService.GoSettings();
        }
    }
}
