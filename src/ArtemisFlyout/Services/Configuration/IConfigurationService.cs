namespace ArtemisFlyout.Services.Configuration
{
    public interface IConfigurationService
    {
        T GetConfig<T>(string settingName, T defaultValue);
        Configuration GetConfiguration();
    }
}
