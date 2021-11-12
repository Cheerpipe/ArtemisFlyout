using System;
using System.Collections.Generic;
using ArtemisFlyout.Events;
using ArtemisFlyout.Models;
using Avalonia.Media;

namespace ArtemisFlyout.Services
{
    //TODO: Split
    public interface IArtemisService
    {
        bool IsExtendedApiRestPluginWorking();
        bool IsJsonDatamodelPluginWorking();
        bool IsRunning();
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
        void SetColor(string colorName, Color color);
        Color GetColor(string colorName, Color defaultColor);
        event EventHandler<ProfileChangeEventArgs> ProfileChanged;
    }
}