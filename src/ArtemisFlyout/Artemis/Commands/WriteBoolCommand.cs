using ArtemisAmbientCli.Commands;
using RestSharp;

namespace ArtemisFlyout.Artemis.Commands
{
    public class WriteBoolCommand : Command
    {
        private string _key;
        private string _jsonPath;

        public WriteBoolCommand(string key, string jsonPath)
        {
            _key = key;
            _jsonPath = jsonPath;
        }


        public void Execute(bool value)
        {
            string request = $"{{{_jsonPath}: {value.ToString().ToLower()} }}";
            var setRequest = CreateRequest(_key, request, Method.PUT);
            ExecuteRequest(setRequest);
        }
    }
}