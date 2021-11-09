using ArtemisFlyout.Models.Configuration;

namespace ArtemisFlyout.Services
{
    public interface IConfigurationService
    {
        Configurations Get();
        void Load();
    }
}
