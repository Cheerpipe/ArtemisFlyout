using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Artemis.Commands;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisMainControlViewModel : ViewModelBase
    {
        public ArtemisMainControlViewModel()
        {
            _ambientProfiles = new List<AmbientProfile>();
            _ambientProfiles.Add(new AmbientProfile { Name = "Cold", Condition = "Cold" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Retrodark", Condition = "Retrodark" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Warm", Condition = "Warm" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Calmwatter", Condition = "Calmwatter" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Retrowave", Condition = "Retrowave" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Comfortable", Condition = "Comfortable" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Relax", Condition = "Relax" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Demo", Condition = "Demo" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Cyberpunk", Condition = "Cyberpunk" });
            _ambientProfiles.Add(new AmbientProfile { Name = "Mario", Condition = "mario" });
            _ambientProfiles.Add(new AmbientProfile { Name = "NES", Condition = "nes" });
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


        private List<AmbientProfile> _ambientProfiles;
        private AmbientProfile? _selectedAmbientProfile;

        private bool _artemisEnabled;

        ReadIntegerCommand readBrightStatus = new ReadIntegerCommand("DesktopVariables", "GlobalBrightness");
        WriteIntCommand writeBrightStatus = new WriteIntCommand("DesktopVariables", "GlobalBrightness");
        public int Bright
        {
            get => readBrightStatus.Execute();
            set => writeBrightStatus.Execute(value);
        }

        public bool ArtemisEnabled
        {
            get => _artemisEnabled;
            set => this.RaiseAndSetIfChanged(ref _artemisEnabled, value);
        }

        public List<AmbientProfile> AmbientProfiles
        {
            get => _ambientProfiles;
            set => this.RaiseAndSetIfChanged(ref _ambientProfiles, value);
        }


        WriteStringCommand writeAmbientProfileName = new WriteStringCommand("DesktopVariables", "Profile");
        ReadCommand readAmbientProfileName = new ReadCommand("DesktopVariables", "Profile");
        public AmbientProfile? SelectedAmbientProfile
        {
            get
            {
                var currentAmbientProfile = readAmbientProfileName.Execute<string>();
                _selectedAmbientProfile = _ambientProfiles.FirstOrDefault(p => p.Condition == currentAmbientProfile);
                return _selectedAmbientProfile;
            }
            set
            {
                _selectedAmbientProfile = value;
                this.RaiseAndSetIfChanged(ref _selectedAmbientProfile, value);
                writeAmbientProfileName.Execute(value?.Condition);
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
