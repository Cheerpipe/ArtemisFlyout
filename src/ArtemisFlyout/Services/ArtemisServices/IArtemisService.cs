﻿
using System.Collections.Generic;
using System.Drawing;

namespace ArtemisFlyout.Services.ArtemisServices
{
    //TODO: Split
    public interface IArtemisService
    {
        void GoHome();
        void GoWorkshop();
        void GoSurfaceEditor();
        void ShowDebugger();
        void GoSettings();
        void RunArtemis();
        void RestartArtemis();
        bool IsArtemisRunning();
        bool SetBright(int value);
        int GetBright();

        bool SetSpeed(int value);
        int GetSpeed();
        public bool SetJsonDataModelValue<T>(string dataModel, string jsonPath, object value);
        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, object defaultValue);
        public List<Profile> GetProfiles(string categoryName = "");
        public bool SetActiveProfile(string profileName);
        public string GetActiveProfile();
    }
}