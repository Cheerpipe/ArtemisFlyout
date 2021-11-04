
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
        bool SetBright(int value);
        int GetBright();
        bool SetSpeed(int value);
        int GetSpeed();
        public bool SetJsonDataModelValue<T>(string dataModel, string jsonPath, T value);
        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, T defaultValue);
        public List<Profile> GetProfiles(string categoryName = "");
        public bool SetActiveProfile(string profileName);
        public string GetActiveProfile();
    }
}