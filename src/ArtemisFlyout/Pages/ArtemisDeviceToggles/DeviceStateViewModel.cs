using ArtemisFlyout.IoC;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;
using System;

namespace ArtemisFlyout.Pages
{
    public class DeviceStateViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly string _devicesStatesDatamodelName;
        private bool _activated;

        public event EventHandler DeviceStateChanged;

        public DeviceStateViewModel(DeviceStateSetting device)
        {
            Name = device.Name;
            Condition = device.Condition;
            _artemisService = Kernel.Get<IArtemisService>();
            _devicesStatesDatamodelName = Kernel.Get<IConfigurationService>().Get().DatamodelSettings.DevicesStatesDatamodelName;
            _activated= _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, Condition, true);
        }

        public bool Activated
        {
            get => _activated;
            set
            {
                _artemisService.SetJsonDataModelValue(_devicesStatesDatamodelName, Condition, value);
                this.RaiseAndSetIfChanged(ref _activated, value);
                DeviceStateChanged?.Invoke(this,EventArgs.Empty);
            }
        }
        public string Name { get; set; }
        public string Condition { get; set; }
    }
}
