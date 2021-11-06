using System.Collections.Generic;
using ArtemisFlyout.Models.Configuration;

namespace ArtemisFlyout.Models.Configuration
{

    public class Configurations
    {
        public LaunchSettings LaunchSettings { get; set; }
        public DatamodelSettings DatamodelSettings { get; set; }
        public RestClientSettings RestClientSettings { get; set; }
        public RestApiSettings RestApiSettings { get; set; }
       public List<Blackout> Blackouts { get; set; }
    }
}
