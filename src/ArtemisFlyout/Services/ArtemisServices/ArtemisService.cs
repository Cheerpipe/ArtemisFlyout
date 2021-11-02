using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArtemisFlyout.JsonDatamodel;
using ArtemisFlyout.Services.Configuration;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Services.RestServices;
using MessageBox.Avalonia.Enums;
using Newtonsoft.Json;

namespace ArtemisFlyout.Services.ArtemisServices
{
    public class ArtemisService : IArtemisService
    {

        private readonly IFlyoutService _flyoutService;
        private readonly IConfigurationService _configurationService;
        private readonly IRestService _restService;

        public ArtemisService(IFlyoutService flyoutService, IConfigurationService configurationService, IRestService restService)
        {
            _flyoutService = flyoutService;
            _configurationService = configurationService;
            _restService = restService;
        }

        public void GoHome()
        {
            _ = _restService.Get("/remote/bring-to-foreground");
            _flyoutService.Close();

        }

        public void GoWorkshop()
        {
            _ = _restService.Get("/windows/show-workshop");
            _flyoutService.Close();
        }

        public void GoSurfaceEditor()
        {
            _ = _restService.Get("/windows/show-surface-editor");
            _flyoutService.Close();
        }

        public void ShowDebugger()
        {
            _ = _restService.Get("/windows/show-debugger");
            _flyoutService.Close();
        }

        public void GoSettings()
        {
            _ = _restService.Get("/windows/show-settings");
            _flyoutService.Close();
        }

        public void RunArtemis()
        {
            Process.Start(
                _configurationService.GetConfiguration().LaunchSettings.ArtemisPath,
                _configurationService.GetConfiguration().LaunchSettings.ArtemisLaunchArgs);
            _flyoutService.Close();
        }

        public async void RestartArtemis()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            _ = _restService.Get("/remote/restart");
            _flyoutService.Close();
        }

        bool IArtemisService.IsArtemisRunning() => Process.GetProcessesByName("Artemis.UI").Length > 0;
        public bool SetBright(int value)
        {
            return SetJsonDataModelValue("DesktopVariables", "GlobalBrightness", value);
        }

        public int GetBright()
        {
            return GetJsonDataModelValue("DesktopVariables", "GlobalBrightness", 0);
        }

        public bool SetSpeed(int value)
        {
            return SetJsonDataModelValue("DesktopVariables", "GlobalSpeed", value);
        }

        public int GetSpeed()
        {
            return GetJsonDataModelValue("DesktopVariables", "GlobalSpeed", 0);
        }

        public bool SetJsonDataModelValue<T>(string dataModel, string jsonPath, T value)
        {
            WriteCommand writeCommand = new WriteCommand(dataModel, jsonPath);
            try
            {
                if (value != null)
                    writeCommand.Execute<T>(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, T defaultValue)
        {
            try
            {
                ReadCommand readCommand = new ReadCommand(dataModel, jsonPath);
                return readCommand.Execute<T>();
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        public List<Profile> GetProfiles(string categoryName = "")
        {
            string restResponse = _restService.Get("/profiles").Trim(new[] { '\uFEFF' });
            return string.IsNullOrEmpty(categoryName) ?
                JsonConvert.DeserializeObject<IEnumerable<Profile>>(restResponse).ToList() :
                JsonConvert.DeserializeObject<IEnumerable<Profile>>(restResponse).Where(p => p.Category.Name == categoryName).ToList();
        }

        public bool SetActiveProfile(string profileName)
        {
            return SetJsonDataModelValue("DesktopVariables", "Profile", profileName);
        }

        public string GetActiveProfile()
        {
            return GetJsonDataModelValue("DesktopVariables", "Profile", "");
        }

        public async void GoToWindow(string windowName)
        {
            await Task.Run(() =>
            {
                _ = _restService.Get($"/windows/{windowName}");
                _flyoutService.Close();
            });
        }
    }
}
