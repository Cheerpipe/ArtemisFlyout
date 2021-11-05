namespace ArtemisFlyout.Services
{
    public interface IConfigurationService
    {
        T GetConfig<T>(string settingName, T defaultValue);
        Configuration GetConfiguration();
    }
}
