using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ArtemisFlyout.Artemis.Commands;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Util;
using MessageBox.Avalonia.Enums;
using Newtonsoft.Json;
using RestSharp;

namespace ArtemisFlyout.Services.ArtemisServices
{
    public class ArtemisService : IArtemisService
    {

        private IFlyoutService _flyoutService;
        public ArtemisService(IFlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
        }

        private const string ArtemisPath = @"D:\Repos\Artemis\src\Artemis.UI\bin\net5.0-windows\Artemis.UI.exe";
        private const string ArtemisParams = "--minimized --pcmr --ignore-plugin-lock";

        public async void GoHome() => RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/bring-to-foreground");

        public async void GoWorkshop()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-workshop");
            _flyoutService.Close();
        }

        public async void GoSurfaceEditor()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-surface-editor");
            _flyoutService.Close();
        }

        public async void ShowDebugger()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-debugger");
            _flyoutService.Close();
        }

        public async void GoSettings()
        {
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/windows/show-settings");
            _flyoutService.Close();
        }

        public async void RunArtemis()
        {
            Process.Start(ArtemisPath, ArtemisParams);
            _flyoutService.Close();
        }

        public async void RestartArtemis()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Artemis", "Are you sure you want restart Artemis?", ButtonEnum.YesNo, Icon.Warning);
            var result = await messageBoxStandardWindow.Show();

            if (result != ButtonResult.Yes)
                return;
            RestUtil.RestGetBool("http://127.0.0.1", 9696, "/remote/restart");
            _flyoutService.Close();
        }

        bool IArtemisService.IsArtemisRunning() => Process.GetProcessesByName("Artemis.UI").Length > 0;
        public bool SetBright(int value)
        {
            return SetJsonDataModelValue<int>("DesktopVariables", "GlobalBrightness", value);
        }

        public int GetBright()
        {
            return GetJsonDataModelValue<int>("DesktopVariables", "GlobalBrightness", 0);
        }

        public bool SetSpeed(int value)
        {
            return SetJsonDataModelValue<int>("DesktopVariables", "GlobalSpeed", value);
        }

        public int GetSpeed()
        {
            return GetJsonDataModelValue<int>("DesktopVariables", "GlobalSpeed", 0);
        }

        public bool SetJsonDataModelValue<T>(string dataModel, string jsonPath, object value)
        {
            WriteCommand writeCommand = new WriteCommand(dataModel, jsonPath);
            try
            {
                if (value is T)
                    writeCommand.Execute<T>(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public T GetJsonDataModelValue<T>(string dataModel, string jsonPath, object defaultValue)
        {
            try
            {
                ReadCommand readCommand = new ReadCommand(dataModel, jsonPath);
                return (T)readCommand.Execute<T>();
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public List<Profile> GetProfiles(string categoryName = "")
        {
            RestClient restClient = new RestClient("http://127.0.0.1:9696");
            restClient.Encoding = Encoding.BigEndianUnicode;
            RestRequest restRequest = new RestRequest("/profiles");
            string restResponse = restClient.Execute(restRequest, restRequest.Method).Content.Trim(new char[] { '\uFEFF' }); ;

            return string.IsNullOrEmpty(categoryName) ? 
                JsonConvert.DeserializeObject<IEnumerable<Profile>>(restResponse).ToList() : 
                JsonConvert.DeserializeObject<IEnumerable<Profile>>(restResponse).Where(p => p.Category.Name == categoryName).ToList();
        }

        public bool SetActiveProfile(string profileName)
        {
            return SetJsonDataModelValue<string>("DesktopVariables", "Profile", profileName);
        }

        public string GetActiveProfile()
        {
            return GetJsonDataModelValue<string>("DesktopVariables", "Profile", "");
        }
    }
}
