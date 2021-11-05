using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtemisFlyout.Services.Configuration;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Services.RestServices;
using MessageBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public bool TestRestApi()
        {
            return (_restService.Get("/profiles").IsSuccessful);
        }

        public void GoHome()
        {
            _ = _restService.Post("/remote/bring-to-foreground");
            _flyoutService.Close();

        }

        public void GoWorkshop()
        {
            _ = _restService.Post("/windows/show-workshop");
            _flyoutService.Close();
        }

        public void GoSurfaceEditor()
        {
            _ = _restService.Post("/windows/show-surface-editor");
            _flyoutService.Close();
        }

        public void ShowDebugger()
        {
            _ = _restService.Post("/windows/show-debugger");
            _flyoutService.Close();
        }

        public void GoSettings()
        {
            _ = _restService.Post("/windows/show-settings");
            _flyoutService.Close();
        }

        public async void RestartArtemis()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            _ = _restService.Post("/remote/restart");
            await _flyoutService.Close();
        }

        public void SetBright(int value)
        {
            SetJsonDataModelValue("DesktopVariables", "GlobalBrightness", value);
        }

        public int GetBright()
        {
            return GetJsonDataModelValue("DesktopVariables", "GlobalBrightness", 0);
        }

        public void SetSpeed(int value)
        {
            SetJsonDataModelValue("DesktopVariables", "GlobalSpeed", value);
        }

        public int GetSpeed()
        {
            return GetJsonDataModelValue("DesktopVariables", "GlobalSpeed", 0);
        }

        public void SetJsonDataModelValue<T>(string dataModel, string jsonPath, T value)
        {
            string content;

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    content = $"{{{jsonPath}: {value.ToString()?.ToLower()} }}";
                    break;
                case TypeCode.String:
                    content = $"{{{jsonPath}: '{value}' }}";
                    break;
                default:
                    content = $"{{{jsonPath}: {value} }}";
                    break;
            }

            string propertyJson = _restService.Put($"/json-datamodel/{dataModel}", content, "application/json").Content;
        }

        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, T defaultValue)
        {
            string propertyJson = _restService.Get($"/json-datamodel/{dataModel}").Content;

            // Create Root and Property
            if (string.IsNullOrEmpty(propertyJson))
            {
                return default(T);
            }

            JObject responseObject = JObject.Parse(propertyJson);
            JToken token = responseObject.SelectToken(jsonPath);
            if (token == null)
            {
                return default(T);
            }

            return token.Value<T>();
        }

        public List<Profile> GetProfiles(string categoryName = "")
        {
            string restResponse = _restService.Get("/profiles").Content.Trim(new[] { '\uFEFF' });

            if (string.IsNullOrEmpty(restResponse)) return new List<Profile>();

            return string.IsNullOrEmpty(categoryName) ?
                JsonConvert.DeserializeObject<IEnumerable<Profile>>(restResponse).ToList() :
                JsonConvert.DeserializeObject<IEnumerable<Profile>>(restResponse).Where(p => p.Category.Name == categoryName).ToList();
        }

        public void SetActiveProfile(string profileName)
        {
            SetJsonDataModelValue("DesktopVariables", "Profile", profileName);
        }

        public string GetActiveProfile()
        {
            return GetJsonDataModelValue("DesktopVariables", "Profile", "");
        }

        public void Launch()
        {
            Process.Start(
                _configurationService.GetConfiguration().LaunchSettings.ArtemisPath,
                _configurationService.GetConfiguration().LaunchSettings.ArtemisLaunchArgs);
        }
    }
}
