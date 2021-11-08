using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtemisFlyout.Events;
using ArtemisFlyout.Models;
using ArtemisFlyout.Utiles;
using Avalonia.Media;
using MessageBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtemisFlyout.Services
{
    public class ArtemisService : IArtemisService
    {
        private readonly IFlyoutService _flyoutService;
        private readonly IConfigurationService _configurationService;
        private readonly string _globalVariablesDatamodelName;
        private readonly IRestService _restService;

        public ArtemisService(IFlyoutService flyoutService, IConfigurationService configurationService, IRestService restService)
        {
            _flyoutService = flyoutService;
            _configurationService = configurationService;
            _globalVariablesDatamodelName = _configurationService.Get().DatamodelSettings.GlobalVariablesDatamodelName;
            _restService = restService;
        }

        public bool TestRestApi()
        {
            return (_restService.Get("/profiles").IsSuccessful);
        }

        public void GoHome()
        {
            _ = _restService.Post("/remote/bring-to-foreground");
            _flyoutService.CloseAndRelease();

        }

        public void GoWorkshop()
        {
            _ = _restService.Post("/windows/show-workshop");
            _flyoutService.CloseAndRelease();
        }

        public void GoSurfaceEditor()
        {
            _ = _restService.Post("/windows/show-surface-editor");
            _flyoutService.CloseAndRelease();
        }

        public void ShowDebugger()
        {
            _ = _restService.Post("/windows/show-debugger");
            _flyoutService.CloseAndRelease();
        }

        public void GoSettings()
        {
            _ = _restService.Post("/windows/show-settings");
            _flyoutService.CloseAndRelease();
        }

        public async void RestartArtemis()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            _ = _restService.Post("/remote/restart");
            await _flyoutService.CloseAndRelease();
        }

        public void SetBright(int value)
        {
            SetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalBrightness", value);
        }

        public int GetBright()
        {
            return GetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalBrightness", 0);
        }

        public void SetSpeed(int value)
        {
            SetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", value);
        }

        public int GetSpeed()
        {
            return GetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalSpeed", 0);
        }

        public void SetJsonDataModelValue<T>(string dataModel, string jsonPath, T value)
        {
            string content = Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Boolean => $"{{{jsonPath}: {value.ToString()?.ToLower()} }}",
                TypeCode.String => $"{{{jsonPath}: '{value}' }}",
                _ => $"{{{jsonPath}: {value} }}"
            };

            _ = _restService.Put($"/json-datamodel/{dataModel}", content, "application/json").Content;
        }

        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, T defaultValue)
        {
            string propertyJson = _restService.Get($"/json-datamodel/{dataModel}").Content;

            // Create Root and Property
            if (string.IsNullOrEmpty(propertyJson))
            {
                return defaultValue;
            }

            try
            {
                JObject responseObject = JObject.Parse(propertyJson);
                JToken token = responseObject.SelectToken(jsonPath);
                if (token == null)
                {
                    return default;
                }

                return token.Value<T>();
            }
            catch (Exception)
            {
                return default;
            }
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
            SetJsonDataModelValue(_globalVariablesDatamodelName, "Profile", profileName);
            ProfileChanged?.Invoke(this, new ProfileChangeEventArgs(profileName));
        }

        public string GetActiveProfile()
        {
            return GetJsonDataModelValue(_globalVariablesDatamodelName, "Profile", "");
        }

        public void Launch()
        {
            Process.Start(
                _configurationService.Get().LaunchSettings.ArtemisPath,
                _configurationService.Get().LaunchSettings.ArtemisLaunchArgs);
        }

        public void SetColor(string colorName, Color color)
        {
            SetJsonDataModelValue(_globalVariablesDatamodelName, colorName, ColorUtiles.ToHexString(color));
        }

        public Color GetColor(string colorName, Color defaultColor)
        {
            if (Color.TryParse(GetJsonDataModelValue(_globalVariablesDatamodelName, colorName, ""), out Color color))
            {
                return color;
            }

            return defaultColor;
        }
        public event EventHandler<ProfileChangeEventArgs> ProfileChanged;
    }
}
