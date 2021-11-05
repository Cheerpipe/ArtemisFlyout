namespace ArtemisFlyout.Services.Configuration
{

    //TODO: Create a model

    public class RestSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
    }
    public class LaunchSettings
    {
        public string ArtemisPath { get; set; }
        public string ArtemisLaunchArgs { get; set; }
    }

    public class DatamodelSettings
    {
        public string BlackoutDatamodelName { get; set; }
        public string DesktopVariablesDatamodelName { get; set; }
    }

    public class Configuration
    {
        public LaunchSettings LaunchSettings { get; set; }
        public DatamodelSettings DatamodelSettings { get; set; }
        public RestSettings RestSettings { get; set; }
    }
}
