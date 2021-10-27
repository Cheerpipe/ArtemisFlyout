using System.Diagnostics;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisViewModel : ViewModelBase
    {
        public bool IsArtemisRunning
        {
            get
            {
                //TODO: Service
                var processName = Process.GetProcessesByName("Artemis.UI");
                return processName.Length != 0;
            }
        }
    }
}
