using AnyConfig;

namespace ArtemisFlyout.Services.Configuration
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

        public Configuration GetConfiguration()
        {
            return Config.Get<Configuration>();
        }
    }
}
