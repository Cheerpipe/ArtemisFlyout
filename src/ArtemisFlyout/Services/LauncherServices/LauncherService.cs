using System;
using System.Diagnostics;
using ArtemisFlyout.Services.Configuration;

namespace ArtemisFlyout.Services.LauncherServices
{
    public class LauncherService : ILauncherService
    {
        private readonly IConfigurationService _configurationService;
        public LauncherService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public bool IsArtemisRunning() => Process.GetProcessesByName("Artemis.UI").Length > 0;

        public bool IsArtemisListening()
        {
            throw new NotImplementedException();
        }

        public void Launch()
        {
            Process.Start(
                 _configurationService.GetConfiguration().LaunchSettings.ArtemisPath,
                 _configurationService.GetConfiguration().LaunchSettings.ArtemisLaunchArgs);
        }
    }
}
