using System;

namespace ArtemisFlyout.Events
{

    public class JsonDataModelValueSentArgs : EventArgs
    {
        internal JsonDataModelValueSentArgs(string dataModel, string jsonPath, object value)
        {
            DataModel = dataModel;
            JsonPath = jsonPath;
            Value = value;
        }

        public string DataModel { get; }
        public string JsonPath { get; }
        public object Value { get; }
    }
}