using ArtemisFlyout.Artemis.Commands;

namespace ArtemisFlyout.Services
{
    public class DeviceService : IDeviceService
    {
        public bool SetState(string deviceName, bool state)
        {
            WriteBoolCommand writeBoolCommand = new WriteBoolCommand("Blackouts", deviceName);
            try
            {
                writeBoolCommand.Execute(state);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetState(string deviceName, bool defaultState = false)
        {
            ReadBoolCommand readBoolCommand = new ReadBoolCommand("Blackouts", deviceName);
            try
            {
                return readBoolCommand.Execute();
            }
            catch
            {
                return false;
            }
        }
    }
}
