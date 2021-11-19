using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using ArtemisFlyout.Events;
using ArtemisFlyout.Models;
using ArtemisFlyout.Utiles;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ArtemisFlyout.Services
{
    public class ArtemisService : IArtemisService
    {
        private readonly IConfigurationService _configurationService;
        private readonly string _globalVariablesDatamodelName;
        private readonly string _keyColorPickerDefaultColorHex;
        private readonly IRestService _restService;

        public ArtemisService(IConfigurationService configurationService, IRestService restService)
        {
            _configurationService = configurationService;
            _globalVariablesDatamodelName = _configurationService.Get().DatamodelSettings.GlobalVariablesDatamodelName;
            _keyColorPickerDefaultColorHex = _configurationService.Get().LedColorPickerLedSettings.DefaultColor;
            _restService = restService;
        }

        public void GoHome()
        {
            _ = _restService.Post("/remote/bring-to-foreground");
        }

        public void GoWorkshop()
        {
            _ = _restService.Post("/windows/show-workshop");
        }

        public void GoSurfaceEditor()
        {
            _ = _restService.Post("/windows/show-surface-editor");
        }

        public void ShowDebugger()
        {
            _ = _restService.Post("/windows/show-debugger");
        }

        public void GoSettings()
        {
            _ = _restService.Post("/windows/show-settings");
        }

        public void RestartArtemis()
        {
            _ = _restService.Post("/remote/restart");
        }

        public void SetBright(int value)
        {
            SetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalBrightness", value);
        }

        public int GetBright()
        {
            return GetJsonDataModelValue(_globalVariablesDatamodelName, "GlobalBrightness", 100);
        }

        public Color GetLedColor(string deviceType, string ledId)
        {
            try
            {
                return Color.Parse(_restService.Get($"/extended-rest-api/get-led-color/{deviceType}/{ledId}").Content);
            }
            catch (Exception)
            {
                if (Color.TryParse(_keyColorPickerDefaultColorHex, out Color defaultColor))
                    return defaultColor;
                return Colors.Transparent;
            }
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
                TypeCode.Boolean => $"{{'{jsonPath}' : {value.ToString()?.ToLower()} }}",
                TypeCode.String => $"{{'{jsonPath}' : '{value}' }}",
                _ => $"{{{jsonPath}: {value} }}"
            };

            _ = _restService.Put($"/json-datamodel/{dataModel}", content, "application/json").Content;
            JsonDataModelValueSent?.Invoke(this, new JsonDataModelValueSentArgs(dataModel, jsonPath, value));
        }

        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, T defaultValue)
        {
            string propertyJson = _restService.Get($"/json-datamodel/{dataModel}").Content;

            // Create Root and Property
            if (string.IsNullOrEmpty(propertyJson))
            {
                SetJsonDataModelValue(dataModel, jsonPath, defaultValue);
                return defaultValue;
            }

            try
            {
                JObject responseObject = JObject.Parse(propertyJson);
                JToken token = responseObject.SelectToken(jsonPath);
                if (token == null)
                {
                    SetJsonDataModelValue(dataModel, jsonPath, defaultValue);
                    return defaultValue;
                }

                return token.Value<T>();
            }
            catch (Exception)
            {
                SetJsonDataModelValue(dataModel, jsonPath, defaultValue);
                return defaultValue;
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
            return GetJsonDataModelValue(_globalVariablesDatamodelName, "Profile", "Default");
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
            if (Color.TryParse(GetJsonDataModelValue(_globalVariablesDatamodelName, colorName, ColorUtiles.ToHexString(Colors.White)), out Color color))
            {
                return color;
            }

            return defaultColor;
        }

        public PluginCheckResult CheckExtendedApiRestPlugin()
        {
            // TODO: Create a more generic plugin checker
            try
            {
                var response = _restService.Get("/extended-rest-api/version");
                var version = response.Content.Substring(2, 7);
                return new PluginCheckResult
                {
                    Installed = true,
                    Responding = response.StatusCode == HttpStatusCode.OK,
                    VersionIsOk = version == Constants.ExtendedRestApiPluginRequiredVersion,
                    RequiredVersion = Constants.ExtendedRestApiPluginRequiredVersion,
                    CurrentVersion = version
                };
            }
            catch
            {
                return new PluginCheckResult
                {
                    Installed = false,
                    Responding = false,
                    VersionIsOk = false,
                    RequiredVersion = Constants.ExtendedRestApiPluginRequiredVersion,
                    CurrentVersion = "0.0.0.0"
                };
            }
        }

        public PluginCheckResult CheckJsonDatamodelPluginPlugin()
        {
            try
            {
                var response = _restService.Get("/json-datamodel/version");
                var version = response.Content.Substring(2, 7);
                return new PluginCheckResult
                {
                    Installed = true,
                    Responding = response.StatusCode == HttpStatusCode.OK,
                    VersionIsOk = version == Constants.JsonDataModelPluginRequiredVersion,
                    RequiredVersion = Constants.JsonDataModelPluginRequiredVersion,
                    CurrentVersion = version
                };
            }
            catch
            {
                return new PluginCheckResult
                {
                    Installed = false,
                    Responding = false,
                    VersionIsOk = false,
                    RequiredVersion = Constants.JsonDataModelPluginRequiredVersion,
                    CurrentVersion = "0.0.0.0"
                };
            }
        }

        public bool IsRunning()
        {
            return Process.GetProcessesByName("Artemis.UI").Length > 0;
        }


        public event EventHandler<ProfileChangeEventArgs> ProfileChanged;
        public event EventHandler<JsonDataModelValueSentArgs> JsonDataModelValueSent;
    }

    // Todo: Move all check logics to a service
    public class PluginCheckResult
    {

        public bool Installed { get; init; }
        public bool Responding { get; init; }
        public bool VersionIsOk { get; init; }
        public string RequiredVersion { get; init; }
        public string CurrentVersion { get; init; }
    }

}
