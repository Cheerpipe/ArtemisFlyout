using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Models;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.Pages
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
            _quickProfile = _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "QuickProfile", false);
            _allDevices = _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, "AllDevices", true);

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
                var currentProfileName = _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "Profile", "Default");
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
            get => _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, "AllDevices", true);
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

        private bool _quickProfile;
        public bool QuickProfile
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "QuickProfile", false);
            set
            {
                _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "QuickProfile", value);
                this.RaiseAndSetIfChanged(ref _quickProfile, value);
            }
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
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", 100);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", value);
        }

    }
}
