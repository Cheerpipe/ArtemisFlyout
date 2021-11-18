using System.Diagnostics;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;

namespace ArtemisFlyout.Pages
{
    public class ArtemisPluginPrerequisitesViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        public ArtemisPluginPrerequisitesViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
        }

        public double FlyoutWindowHeight => 280;
        public double FlyoutWindowWidth => 550;

        public double FlyoutWidth => FlyoutWindowWidth - 12;
        public double FlyoutHeight => FlyoutWindowHeight - 12;
        public int ActivePageindex { get; } = 4;
        public PluginStateViewModel JsonDatamodelPluginState
        {
            get
            {
                PluginCheckResult state = _artemisService.CheckJsonDatamodelPluginPlugin();

                bool responding = state.Responding;
                bool versionOk = state.VersionIsOk;
                string stateMessage;

                if (versionOk)
                    stateMessage = "Json plugin installed and running";
                else if (responding)
                    stateMessage = $"Json plugin version {state.RequiredVersion} is required but {state.CurrentVersion} is installed";
                else
                    stateMessage = "Json plugin not running or not installed";

                return new PluginStateViewModel
                {
                    State = versionOk,
                    StateText = stateMessage
                };
            }
        }

        public PluginStateViewModel ExtendedRestApiPluginState
        {
            get
            {
                PluginCheckResult state = _artemisService.CheckExtendedApiRestPlugin();

                bool responding = state.Responding;
                bool versionOk = state.VersionIsOk;
                string stateMessage;

                if (versionOk)
                    stateMessage = "Extended REST Api plugin installed and running";
                else if (responding)
                {
                    stateMessage = $"Extended REST Api plugin version {state.RequiredVersion} is required but {state.CurrentVersion} is installed";
                }
                else
                {
                    stateMessage = "Extended REST Api plugin not running or not installed";
                }

                return new PluginStateViewModel
                {
                    State = versionOk,
                    StateText = stateMessage
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
