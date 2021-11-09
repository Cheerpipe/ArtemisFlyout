using ArtemisFlyout.IoC;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.Pages
{
    public class DeviceStateViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly string _devicesStatesDatamodelName;
        private bool _activated;

        public DeviceStateViewModel(DeviceStateSetting device)
        {
            Name = device.Name;
            Condition = device.Condition;
            _artemisService = Kernel.Get<IArtemisService>();
            _devicesStatesDatamodelName = Kernel.Get<IConfigurationService>().Get().DatamodelSettings.DevicesStatesDatamodelName;
        }

        public bool Activated
        {
            get => _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, Condition, false);
            set
            {
                _artemisService.SetJsonDataModelValue(_devicesStatesDatamodelName, Condition, value);
                this.RaiseAndSetIfChanged(ref _activated, value);
            }
        }
        public string Name { get; set; }
        public string Condition { get; set; }
    }
}
