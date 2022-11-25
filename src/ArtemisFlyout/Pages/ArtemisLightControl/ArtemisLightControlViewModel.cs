using System;
using System.Collections.Generic;
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
        private readonly string _devicesStatesDatamodelName;
        private readonly string _globalVariablesDatamodelName;
        private readonly string _ambientProfileCategoryName;
        private readonly List<QuickActionViewModel> _quickAcionsViewModels;
        private List<Profile> _profiles;
        private Profile _selectedProfile;

        public ArtemisLightControlViewModel(IArtemisService artemisService, IConfigurationService configurationService, List<QuickActionViewModel> quickAcionsViewModels)
        {
            _artemisService = artemisService;
            _quickAcionsViewModels = quickAcionsViewModels;
            var configurationService1 = configurationService;
            _devicesStatesDatamodelName = configurationService1.Get().DatamodelSettings.DevicesStatesDatamodelName;
            var quickActions = configurationService1.Get().QuickActions;
            _globalVariablesDatamodelName = configurationService1.Get().DatamodelSettings.GlobalVariablesDatamodelName;
            _ambientProfileCategoryName = configurationService1.Get().DatamodelSettings.AmbientProfileCategoryName;
            _quickProfile = _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "QuickProfile", false);
            _allDevices = _artemisService.GetJsonDataModelValue(_devicesStatesDatamodelName, "AllDevices", true);

            // TODO: Better DAL
            // Workaround for initialize this valua that is the only one not being readed by the UI.
            _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "DeviceSettingsOverride", false);

            foreach (var qa in quickActions)
            {
                _quickAcionsViewModels.Add(
                    new QuickActionViewModelBuilder().
                        WithName(qa.Name)
                        .WithCondition(qa.Condition)
                        .WithIcon(qa.Icon)
                        .Build()
                    );
            }

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

        public List<QuickActionViewModel> QuickActionsVms => _quickAcionsViewModels;

        public List<Profile> Profiles => _profiles = _artemisService.GetProfiles(_ambientProfileCategoryName);

        public event EventHandler SelectedProfileChanged;
        public Profile SelectedProfile
        {
            get
            {
                var currentProfileName = _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "Profile", "Default");
                return _selectedProfile = _profiles.FirstOrDefault(p => p.Name == currentProfileName);
            }
            set
            {
                _artemisService.SetActiveProfile(value?.Name);
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
                SelectedProfileChanged?.Invoke(this, EventArgs.Empty);
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

        public async void ToggleDeviceSettingsOverride(bool state)
        {
            await System.Threading.Tasks.Task.Delay(state ? 200 : 0); // Give time to the special profile to start up but disable override as son as the user click the button.
            _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "DeviceSettingsOverride", state);
        }

        public void DisableDeviceSettingsOverride()
        {
            ToggleDeviceSettingsOverride(false);
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

        public int Speed
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", 100);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", value);
        }

    }
}
