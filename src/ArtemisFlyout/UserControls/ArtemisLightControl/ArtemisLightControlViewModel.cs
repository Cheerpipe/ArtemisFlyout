using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Models;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public class ArtemisLightControlViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private List<Profile> _profiles;
        private readonly string _devicesStatesDatamodelName;
        private readonly string _globalVariablesDatamodelName;
        private Profile _selectedProfile;

        public ArtemisLightControlViewModel(IArtemisService artemisService, IConfigurationService configurationService)
        {
            _artemisService = artemisService;
            var configurationService1 = configurationService;
            _devicesStatesDatamodelName = configurationService1.Get().DatamodelSettings.DevicesStatesDatamodelName;
            _globalVariablesDatamodelName= configurationService1.Get().DatamodelSettings.GlobalVariablesDatamodelName;
            _allDevices = _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, "AllDevices", false);

            this.WhenActivated(disposables =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);
            });
        }

        public int Bright
        {
            get => _artemisService.GetBright();
            set => _artemisService.SetBright(value);
        }

        public List<Profile> Profiles => _profiles = _artemisService.GetProfiles("Ambient");

        public Profile SelectedProfile
        {
            get
            {
                var currentProfileName = _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "Profile", "");
                return _selectedProfile = _profiles.FirstOrDefault(p => p.Name == currentProfileName);
            }
            set
            {
                _artemisService.SetActiveProfile(value.Name);
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
            }
        }

        private bool _allDevices;
        public bool AllDevices
        {
            get => _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, "AllDevices", false);
            set
            {
                _artemisService.SetJsonDataModelValue(_devicesStatesDatamodelName, "AllDevices", value);
                this.RaiseAndSetIfChanged(ref _allDevices, value);
            }
        }

        public bool Webcam
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "Webcam", false);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "Webcam", value);
        }

        public bool QuickProfile
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "QuickProfile", false);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "QuickProfile", value);
        }

        public bool Ambilight
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "Ambilight", false);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "Ambilight", value);
        }

        public bool AudioReactive
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "AudioReactive", false);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "AudioReactive", value);
        }


        public int Speed
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", 0);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", value);
        }
    }
}
