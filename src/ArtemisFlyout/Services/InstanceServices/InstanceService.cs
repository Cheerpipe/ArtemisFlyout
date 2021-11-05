using System.Threading;

namespace ArtemisFlyout.Services
{
    public class InstanceService : IInstanceService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRestService _restService;
        public InstanceService(IConfigurationService configurationService, IRestService restService)
        {
            _restService = restService;
            _configurationService = configurationService;

        }

        public Mutex InstanceMutex { get; private set; }

        public bool IsAlreadyRunning()
        {
            InstanceMutex = new Mutex(true, "29c8ae9a-af5c-4751-ad5e-98f11397cfb4", out bool created);
            return !created;
        }

        public void ShowInstanceFlyout()
        {
            _restService.Get(
                "http://127.0.0.1",
                _configurationService.GetConfiguration().RestApiSettings.Port,
                "/flyout/show");
        }
    }
}
