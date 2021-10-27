using System;
using ArtemisAmbientCli.Commands;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ArtemisFlyout.Artemis.Commands
{
    public class ReadStringCommand : Command
    {
        private string _key;
        private string _jsonPath;

        public ReadStringCommand(string key, string jsonPath)
        {
            _key = key;
            _jsonPath = jsonPath;
        }

        public string Execute()
        {
            var getRequest = CreateRequest(_key, _key, Method.GET);
            string propertyJson = ExecuteRequest(getRequest);

            // Create Root and Property
            if (string.IsNullOrEmpty(propertyJson))
            {
                throw new Exception("Can't read JSON Datamodel");
            }

            JObject responseObject = JObject.Parse(propertyJson);
            JToken? token = responseObject.SelectToken(_jsonPath);
            if (token == null)
            {
                throw new Exception("Value don't exists");
            }

            return token.Value<string>();
        }
    }
}