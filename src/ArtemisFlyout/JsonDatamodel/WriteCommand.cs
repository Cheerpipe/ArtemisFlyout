using System;
using RestSharp;

namespace ArtemisFlyout.JsonDatamodel
{
    public class WriteCommand : Command
    {
        private string _dataModel;
        private string _jsonPath;

        public WriteCommand(string dataModel, string jsonPath)
        {
            _dataModel = dataModel;
            _jsonPath = jsonPath;
        }

        public void Execute<T>(object value)
        {
            string request;


            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    request = $"{{{_jsonPath}: {value.ToString()?.ToLower()} }}";
                    break;
                case TypeCode.String:
                    request = $"{{{_jsonPath}: '{value}' }}";
                    break;
                default:
                    request = $"{{{_jsonPath}: {value} }}";
                    break;
            }

            var setRequest = CreateRequest(_dataModel, request, Method.PUT);
            ExecuteRequest(setRequest);
        }
    }
}