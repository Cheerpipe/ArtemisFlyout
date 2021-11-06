using System.IO;
using ArtemisFlyout.Models.Configuration;
using Newtonsoft.Json;

namespace ArtemisFlyout.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private Configurations _configurations;
        public ConfigurationService()
        {
            Load();
        }

        public void Load()
        {
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string appsettingsString = r.ReadToEnd();
                _configurations = JsonConvert.DeserializeObject<Configurations>(appsettingsString);
            }
        }

        public Configurations Get() => _configurations;

    }
}
