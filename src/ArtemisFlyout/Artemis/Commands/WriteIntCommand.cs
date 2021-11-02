using RestSharp;

namespace ArtemisFlyout.Artemis.Commands
{
    public class WriteIntCommand : Command
    {
        private string _key;
        private string _jsonPath;

        public WriteIntCommand(string key, string jsonPath)
        {
            _key = key;
            _jsonPath = jsonPath;
        }


        public void Execute(int value)
        {
            string request = $"{{{_jsonPath}: {value.ToString().ToLower()} }}";
            var setRequest = CreateRequest(_key, request, Method.PUT);
            ExecuteRequest(setRequest);
        }
    }
}