
using System.Collections.Generic;

namespace ArtemisFlyout.Services.ArtemisServices
{
    //TODO: Split
    public interface IArtemisService
    {
        bool TestRestApi();
        void GoHome();
        void GoWorkshop();
        void GoSurfaceEditor();
        void ShowDebugger();
        void GoSettings();
        void RestartArtemis();
        void SetBright(int value);
        int GetBright();
        void SetSpeed(int value);
        int GetSpeed();
        public void SetJsonDataModelValue<T>(string dataModel, string jsonPath, T value);
        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, T defaultValue);
        public List<Profile> GetProfiles(string categoryName = "");
        public void SetActiveProfile(string profileName);
        public string GetActiveProfile();
        void Launch();
    }
}