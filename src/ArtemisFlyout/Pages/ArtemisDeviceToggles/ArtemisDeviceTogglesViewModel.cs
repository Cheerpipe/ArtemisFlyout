﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.Pages
{

    public class ArtemisDeviceTogglesViewModel : ViewModelBase
    {
        private readonly List<DeviceStateViewModel> _devicesStates;

        public ArtemisDeviceTogglesViewModel(IConfigurationService configurationService)
        {
            var devicesStatesSettings = configurationService.Get().DevicesStatesSettings;

            _devicesStates = new List<DeviceStateViewModel>();

            foreach (var deviceStateVm in devicesStatesSettings.Select(deviceState => new DeviceStateViewModel(deviceState)))
            {
                deviceStateVm.DeviceStateChanged += DevStateVM_DeviceStateChanged;
                _devicesStates.Add(deviceStateVm);
            }

            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() =>
                    {
                        foreach (var deviceStateVm in devicesStatesSettings.Select(deviceState => new DeviceStateViewModel(deviceState)))
                        {
                            deviceStateVm.DeviceStateChanged -= DevStateVM_DeviceStateChanged;
                        }
                    })
                    .DisposeWith(disposables);
            });
        }

        private void DevStateVM_DeviceStateChanged(object sender, EventArgs e)
        {
            this.RaisePropertyChanged(nameof(All));
        }

        public double CalculatedHeight => (46 * (DeviceStates.Count + 1)) + 150;

        public event EventHandler AllStateChanged;

        public bool All
        {
            get => GetAllTogglesState();
            set
            {
                SetAllTogglesStates(value);
                AllStateChanged?.Invoke(this, EventArgs.Empty);
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
            return DeviceStates.Any(dev => dev.Activated);
        }

        public List<DeviceStateViewModel> DeviceStates => _devicesStates;
    }
}
