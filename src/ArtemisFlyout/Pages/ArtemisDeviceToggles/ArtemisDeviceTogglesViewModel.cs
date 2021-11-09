using System.Collections.Generic;
using System.Reactive.Disposables;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.Pages
{

    public class ArtemisDeviceTogglesViewModel : ViewModelBase
    {
        private readonly List<DeviceStateSetting> _devicesStatesSettings;
        private List<DeviceStateViewModel> _devicesStates;

        public ArtemisDeviceTogglesViewModel(IConfigurationService configurationService)
        {
             _devicesStatesSettings = configurationService.Get().DevicesStatesSettings;

            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() =>
                    {
                    })
                    .DisposeWith(disposables);
            });
        }

        public List<DeviceStateViewModel> DeviceStates
        {
            get
            {
                _devicesStates = new();
                foreach (var deviceState in _devicesStatesSettings)
                {
                    _devicesStates.Add(new DeviceStateViewModel(deviceState));
                }
                return _devicesStates;
            }
        }
    }
}
