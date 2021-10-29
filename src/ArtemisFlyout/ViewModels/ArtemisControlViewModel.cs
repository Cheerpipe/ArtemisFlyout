using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtemisFlyout.Artemis.Commands;
using ArtemisFlyout.Util;

using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisControlViewModel : ViewModelBase
    {
        ReadBoolCommand readBlackoutStatus = new ReadBoolCommand("Blackouts", "FullBlackout");
        WriteBoolCommand writeBlackoutStatus = new WriteBoolCommand("Blackouts", "FullBlackout");

        ReadBoolCommand readMoonLightStatus = new ReadBoolCommand("Blackouts", "MoonBlackout");
        WriteBoolCommand writeMoonLightStatus = new WriteBoolCommand("Blackouts", "MoonBlackout");

        ReadIntegerCommand readBrightStatus = new ReadIntegerCommand("DesktopVariables", "GlobalBrightness");
        WriteIntCommand writeBrightStatus = new WriteIntCommand("DesktopVariables", "GlobalBrightness");

        ReadBoolCommand readTeamsStatus = new ReadBoolCommand("DesktopVariables", "TeamsLight");
        WriteBoolCommand writeTeamsStatus = new WriteBoolCommand("DesktopVariables", "TeamsLight");

        ReadBoolCommand readAudioReactiveStatus = new ReadBoolCommand("DesktopVariables", "AudioReactive");
        WriteBoolCommand writeAudioReactiveStatus = new WriteBoolCommand("DesktopVariables", "AudioReactive");

        ReadBoolCommand readAmbiLightStatus = new ReadBoolCommand("DesktopVariables", "AmbiLight");
        WriteBoolCommand writeAmbiLightStatus = new WriteBoolCommand("DesktopVariables", "AmbiLight");

        WriteStringCommand writeAmbientProfileName = new WriteStringCommand("DesktopVariables", "Profile");
        ReadCommand readAmbientProfileName = new ReadCommand("DesktopVariables", "Profile");

        private List<AmbientProfile> _ambientProfiles;
        private AmbientProfile? _selectedAmbientProfile;

        private bool _artemisEnabled;

        public ArtemisControlViewModel()
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
            _artemisEnabled = !readBlackoutStatus.Execute();
        }

        public bool Blackout
        {
            get => readBlackoutStatus.Execute();
            set
            {
                writeBlackoutStatus.Execute(value);
                ArtemisEnabled = !value;
            }
        }

        public bool MoonLight
        {
            get => !readMoonLightStatus.Execute();
            set => writeMoonLightStatus.Execute(!value);
        }

        public int Bright
        {
            get => readBrightStatus.Execute();
            set => writeBrightStatus.Execute(value);
        }

        public object ComboBoxItems => new List<string>();

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

        public static bool IsRunning()
        {
            var processName = Process.GetProcessesByName("Artemis.UI");
            return processName.Length != 0;
        }
    }
}
