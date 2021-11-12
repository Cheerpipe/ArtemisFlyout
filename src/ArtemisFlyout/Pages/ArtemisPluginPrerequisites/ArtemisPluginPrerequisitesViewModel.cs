using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using System.Diagnostics;

namespace ArtemisFlyout.Pages
{
    public class ArtemisPluginPrerequisitesViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        public ArtemisPluginPrerequisitesViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
        }

        public double FlyoutHeight => 280;
        public double FlyoutWidth => 420;
        public int ActivePageindex { get; } = 4;
        public PluginStateViewModel JsonDatamodelPluginState
        {
            get
            {
                bool state = _artemisService.IsJsonDatamodelPluginWorking();
                return new PluginStateViewModel()
                {
                    State = state,
                    StateText = state ? "Json plugin installed and running" : "Json plugin not running"
                };
            }
        }


        public PluginStateViewModel ExtendedRestApiPluginState
        {
            get
            {
                bool state = _artemisService.IsExtendedApiRestPluginWorking();
                return new PluginStateViewModel()
                {

                    State = state,
                    StateText = state ? "Extended Web API plugin installed and running" : "Extended Web API plugin not installed or not running"
                };
            }
        }

        public bool CommandBarVisibleState => false;

        public void GoToDownloads()
        {
            var ps = new ProcessStartInfo(@"https://nightly.link/Cheerpipe/Artemis.Plugins.Public/workflows/plugins/master")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }
    }

    public class PluginStateViewModel
    {
        public bool State { get; set; }
        public string StateText { get; set; }

    }
}
