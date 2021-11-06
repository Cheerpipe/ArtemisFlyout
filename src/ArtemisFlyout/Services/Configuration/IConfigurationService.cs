using ArtemisFlyout.Models;

namespace ArtemisFlyout.Services
{
    public interface IConfigurationService
    {
        T GetConfig<T>(string settingName, T defaultValue);
        Configurations Get();
    }
}
