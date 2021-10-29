using ArtemisFlyout.Artemis.Commands;

namespace ArtemisFlyout.ViewModels
{
    public class DeviceTogglesViewModel : ViewModelBase
    {
        public DeviceTogglesViewModel()
        {

        }
        ReadBoolCommand readCeilingState = new ReadBoolCommand("Blackouts", "MoonBlackout");
        WriteBoolCommand writeCeilingState = new WriteBoolCommand("Blackouts", "MoonBlackout");
        public bool Ceiling
        {
            get
            {
                return readCeilingState.Execute();
            }
            set
            {
                writeCeilingState.Execute(value);
            }
        }


        ReadBoolCommand readTelevisionState = new ReadBoolCommand("Blackouts", "TvBlackout");
        WriteBoolCommand writeTelevisionState = new WriteBoolCommand("Blackouts", "TvBlackout");
        public bool Television
        {
            get => readTelevisionState.Execute();
            set => writeTelevisionState.Execute(value);
        }

        ReadBoolCommand readDisplayState = new ReadBoolCommand("Blackouts", "DisplayBlackout");
        WriteBoolCommand writeDisplayState = new WriteBoolCommand("Blackouts", "DisplayBlackout");
        public bool Display
        {
            get => readDisplayState.Execute();
            set => writeDisplayState.Execute(value);
        }

        ReadBoolCommand readSpeakerState = new ReadBoolCommand("Blackouts", "SpeakerBlackout");
        WriteBoolCommand writeSpeakerState = new WriteBoolCommand("Blackouts", "SpeakerBlackout");
        public bool Speakers
        {
            get => readSpeakerState.Execute();
            set => writeSpeakerState.Execute(value);
        }

        ReadBoolCommand readTableBackState = new ReadBoolCommand("Blackouts", "TableBackBlackout");
        WriteBoolCommand writeTableBackState = new WriteBoolCommand("Blackouts", "TableBackBlackout");
        public bool DesktopBackside
        {
            get => readTableBackState.Execute();
            set => writeTableBackState.Execute(value);
        }

        ReadBoolCommand readPeripheralsState = new ReadBoolCommand("Blackouts", "PeripheralsBlackout");
        WriteBoolCommand writePeripheralsState = new WriteBoolCommand("Blackouts", "PeripheralsBlackout");
        public bool Peripherals
        {
            get => readPeripheralsState.Execute();
            set => writePeripheralsState.Execute(value);
        }

        ReadBoolCommand readDesktopSurfaceState = new ReadBoolCommand("Blackouts", "TableOverBlackout");
        WriteBoolCommand writeDesktopSurfaceState = new WriteBoolCommand("Blackouts", "TableOverBlackout");
        public bool DesktopSurface
        {
            get => readDesktopSurfaceState.Execute();
            set => writeDesktopSurfaceState.Execute(value);
        }

        ReadBoolCommand readDesktopUndersideState = new ReadBoolCommand("Blackouts", "TableUnderBlackout");
        WriteBoolCommand writeDesktopUndersideState = new WriteBoolCommand("Blackouts", "TableUnderBlackout");
        public bool DesktopUnderside
        {
            get => readDesktopUndersideState.Execute();
            set => writeDesktopUndersideState.Execute(value);
        }

        ReadBoolCommand readNightTableState = new ReadBoolCommand("Blackouts", "NightTableBlackout");
        WriteBoolCommand writeNightTableState = new WriteBoolCommand("Blackouts", "NightTableBlackout");
        public bool NightTable
        {
            get => readNightTableState.Execute();
            set => writeNightTableState.Execute(value);
        }

        ReadBoolCommand readComputerCaseState = new ReadBoolCommand("Blackouts", "ComputerCaseBlackout");
        WriteBoolCommand writeComputerCaseState = new WriteBoolCommand("Blackouts", "ComputerCaseBlackout");
        public bool ComputerCase
        {
            get => readComputerCaseState.Execute();
            set => writeComputerCaseState.Execute(value);
        }
    }
}
