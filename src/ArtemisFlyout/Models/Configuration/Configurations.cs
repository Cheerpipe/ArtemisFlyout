using System.Collections.Generic;

namespace ArtemisFlyout.Models
{

    public class Configurations
    {
        public LaunchSettings LaunchSettings { get; set; }
        public DatamodelSettings DatamodelSettings { get; set; }
        public RestClientSettings RestClientSettings { get; set; }
        public RestApiSettings RestApiSettings { get; set; }
        public List<BlackoutSetting> BlackoutSettings { get; set; }
    }
}
