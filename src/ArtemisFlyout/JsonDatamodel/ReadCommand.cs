using System;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ArtemisFlyout.JsonDatamodel
{
    public class ReadCommand : Command
    {
        private string _key;
        private string _jsonPath;

        public ReadCommand(string key, string jsonPath)
        {
            _key = key;
            _jsonPath = jsonPath;
        }

        public T Execute<T>()
        {
            var getRequest = CreateRequest(_key, _key, Method.GET);
            string propertyJson = ExecuteRequest(getRequest);

            // Create Root and Property
            if (string.IsNullOrEmpty(propertyJson))
            {
                return default(T);
            }
            
            JObject responseObject = JObject.Parse(propertyJson);
            JToken token = responseObject.SelectToken(_jsonPath);
            if (token == null)
            {
                return default(T);
            }

            return token.Value<T>();
        }
    }
}