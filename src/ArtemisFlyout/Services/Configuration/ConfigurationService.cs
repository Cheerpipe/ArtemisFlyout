using AnyConfig;
using Configurations = ArtemisFlyout.Models.Configurations;

namespace ArtemisFlyout.Services
{
    public class ConfigurationService: IConfigurationService
    {

        public ConfigurationService()
        {
            ConfigurationManager.ConfigurationFilename = "settings.json";
        }

        public T GetConfig<T>(string settingName, T defaultValue)
        {
            return  Config.Get(settingName,defaultValue);
        }

        public Configurations Get()
        {
            return Config.Get<Configurations>();
        }
    }
}
