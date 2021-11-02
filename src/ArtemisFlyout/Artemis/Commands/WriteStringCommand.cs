using RestSharp;

namespace ArtemisFlyout.Artemis.Commands
{
    public class WriteStringCommand : Command
    {
        private string _key;
        private string _jsonPath;

        public WriteStringCommand(string key, string jsonPath)
        {
            _key = key;
            _jsonPath = jsonPath;
        }


        public void Execute(string? value)
        {
            string request = $"{{{_jsonPath}: \"{value}\" }}";
            var setRequest = CreateRequest(_key, request, Method.PUT);
            ExecuteRequest(setRequest);
        }

        public void Execute(string value, string jsonPath)
        {
            string request = $"{{{jsonPath}: \"{value}\" }}";
            var setRequest = CreateRequest(_key, request, Method.PUT);
            ExecuteRequest(setRequest);
        }
    }
}