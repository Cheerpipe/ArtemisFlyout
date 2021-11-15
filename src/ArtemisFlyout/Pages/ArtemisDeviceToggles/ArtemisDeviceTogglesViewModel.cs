using System;
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

            _devicesStates = new();

            foreach (var deviceState in _devicesStatesSettings)
            {
                var deviceStateVm = new DeviceStateViewModel(deviceState);
                deviceStateVm.DeviceStateChanged += DevStateVM_DeviceStateChanged;
                _devicesStates.Add(deviceStateVm);
            }

            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() =>
                    {

                    })
                    .DisposeWith(disposables);
            });
        }

        private void DevStateVM_DeviceStateChanged(object sender, EventArgs e)
        {
            this.RaisePropertyChanged("All");
        }

        public double CalculatedHeight => (46 * (DeviceStates.Count + 1)) + 150;

        public event EventHandler AllStateChanged;

        public bool All
        {
            get
            {
                return GetAllTogglesState();
            }
            set
            {
                SetAllTogglesStates(value);
                AllStateChanged?.Invoke(this, new EventArgs());
            }
        }



        private void SetAllTogglesStates(bool state)
        {
            foreach (var dev in DeviceStates)
            {
                dev.Activated = state;
            }
        }

        private bool GetAllTogglesState()
        {
            foreach (var dev in DeviceStates)
            {
                if (dev.Activated)
                    return true;
            }
            return false;
        }

        public List<DeviceStateViewModel> DeviceStates
        {
            get
            {
                return _devicesStates;
            }
        }
    }
}
