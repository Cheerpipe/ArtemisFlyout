using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Services.ArtemisServices;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisMainControlViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly List<Profile> _profiles;
        private Profile _selectedProfile;

        public ArtemisMainControlViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
            _profiles = _artemisService.GetProfiles("Ambient");
            _fullBlackout = _artemisService.GetJsonDataModelValue("Blackouts", "FullBlackout", false);

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

        public List<Profile> Profiles => _profiles;

        public Profile SelectedProfile
        {
            get
            {
                var currentProfileName = _artemisService.GetJsonDataModelValue("DesktopVariables", "Profile", "");
                return _selectedProfile = _profiles.FirstOrDefault(p => p.Name == currentProfileName);
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
                _artemisService.SetJsonDataModelValue("DesktopVariables", "Profile", value.Name);
            }
        }

        private bool _fullBlackout;
        public bool FullBlackout
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "FullBlackout", false);
            set
            {
                _artemisService.SetJsonDataModelValue("Blackouts", "FullBlackout", value);
                this.RaiseAndSetIfChanged(ref _fullBlackout, value);
            }
        }

        public bool Teams
        {
            get => _artemisService.GetJsonDataModelValue("DesktopVariables", "TeamsLight", false);
            set => _artemisService.SetJsonDataModelValue("DesktopVariables", "TeamsLight", value);
        }

        public bool AmbiLight
        {
            get => _artemisService.GetJsonDataModelValue("DesktopVariables", "AmbiLight", false);
            set => _artemisService.SetJsonDataModelValue("DesktopVariables", "AmbiLight", value);
        }

        public bool AudioReactive
        {
            get => _artemisService.GetJsonDataModelValue("DesktopVariables", "AudioReactive", false);
            set => _artemisService.SetJsonDataModelValue("DesktopVariables", "AudioReactive", value);
        }


        public int Speed
        {
            get => _artemisService.GetJsonDataModelValue("DesktopVariables", "GlobalSpeed", 0);
            set => _artemisService.SetJsonDataModelValue("DesktopVariables", "GlobalSpeed", value);
        }
    }
}
