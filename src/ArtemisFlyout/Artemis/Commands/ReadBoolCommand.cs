using System;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ArtemisFlyout.Artemis.Commands
{
    public class ReadBoolCommand : Command
    {
        private string _key;
        private string _jsonPath;

        public ReadBoolCommand(string key, string jsonPath)
        {
            _key = key;
            _jsonPath = jsonPath;
        }

        public bool Execute()
        {
            var getRequest = CreateRequest(_key, _key, Method.GET);
            string propertyJson = ExecuteRequest(getRequest);

            // Create Root and Property
            if (string.IsNullOrEmpty(propertyJson))
            {
                throw new Exception("Cant read JSON Datamodel");
            }

            JObject responseObject = JObject.Parse(propertyJson);
            JToken? token = responseObject.SelectToken(_jsonPath);

            if (token == null)
            {
                throw new Exception("Value don't exists");
            }

            return token.Value<bool>();
        }
    }
}
