using System.Diagnostics;

namespace ArtemisFlyout.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _contentPage;
        public bool IsArtemisRunning => Process.GetProcessesByName("Artemis.UI").Length > 0;

        public int ContentPageIndex
        {
            get
            {
                if (!IsArtemisRunning)
                    return 1;
                else
                    return 0;
            }
        }
    }
}
