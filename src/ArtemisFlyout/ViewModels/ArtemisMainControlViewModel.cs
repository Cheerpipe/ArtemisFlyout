using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Artemis.Commands;
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
            _fullBlackout = _artemisService.GetJsonDataModelValue<bool>("Blackouts", "FullBlackout", false);
            // _artemisEnabled = !readBlackoutStatus.Execute();

            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() =>
                    {
                    })
                    .DisposeWith(disposables);
            });
        }

        public int Bright
        {
            get => _artemisService.GetBright();
            set => _artemisService.SetBright(value);
        }

        public List<Profile> Profiles
        {
            get => _profiles;
        }

        public Profile SelectedProfile
        {
            get
            {
                var currentProfileName = _artemisService.GetJsonDataModelValue<string>("DesktopVariables", "Profile", "");
                return _selectedProfile = _profiles.FirstOrDefault(p => p.Name == currentProfileName);
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
                _artemisService.SetJsonDataModelValue<string>("DesktopVariables", "Profile", value.Name);
            }
        }

        private bool _fullBlackout;
        public bool FullBlackout
        {
            get
            {
                return _artemisService.GetJsonDataModelValue<bool>("Blackouts", "FullBlackout", _fullBlackout);
            }
            set
            {
                _artemisService.SetJsonDataModelValue<bool>("Blackouts", "FullBlackout", value);
                this.RaiseAndSetIfChanged(ref _fullBlackout, value);
            }
        }


        ReadBoolCommand readTeamsStatus = new ReadBoolCommand("DesktopVariables", "TeamsLight");
        WriteBoolCommand writeTeamsStatus = new WriteBoolCommand("DesktopVariables", "TeamsLight");
        public bool Teams
        {
            get
            {
                return readTeamsStatus.Execute();
            }
            set
            {
                writeTeamsStatus.Execute(value);
            }
        }

        ReadBoolCommand readAmbiLightStatus = new ReadBoolCommand("DesktopVariables", "AmbiLight");
        WriteBoolCommand writeAmbiLightStatus = new WriteBoolCommand("DesktopVariables", "AmbiLight");
        public bool AmbiLight
        {
            get
            {
                return readAmbiLightStatus.Execute();
            }
            set
            {
                writeAmbiLightStatus.Execute(value);
            }
        }


        ReadBoolCommand readAudioReactiveStatus = new ReadBoolCommand("DesktopVariables", "AudioReactive");
        WriteBoolCommand writeAudioReactiveStatus = new WriteBoolCommand("DesktopVariables", "AudioReactive");
        public bool AudioReactive
        {
            get
            {
                return readAudioReactiveStatus.Execute();
            }
            set
            {
                writeAudioReactiveStatus.Execute(value);
            }
        }

        ReadIntegerCommand readSpeedStatus = new ReadIntegerCommand("DesktopVariables", "GlobalSpeed");
        WriteIntCommand writeSpeedStatus = new WriteIntCommand("DesktopVariables", "GlobalSpeed");
        public int Speed
        {
            get => readSpeedStatus.Execute();
            set => writeSpeedStatus.Execute(value);
        }

        public static bool IsRunning()
        {
            var processName = Process.GetProcessesByName("Artemis.UI");
            return processName.Length != 0;
        }
    }
}
