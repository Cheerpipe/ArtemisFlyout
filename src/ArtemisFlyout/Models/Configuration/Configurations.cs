using System.Collections.Generic;

namespace ArtemisFlyout.Models.Configuration
{

    public class Configurations
    {
        public LaunchSettings LaunchSettings { get; set; }
        public DatamodelSettings DatamodelSettings { get; set; }
        public RestClientSettings RestClientSettings { get; set; }
        public RestApiSettings RestApiSettings { get; set; }
       public List<DeviceStateSetting> DevicesStatesSettings { get; set; }
       public List<CustomProfileColorSetting> CustomProfilesColorSettings { get; set; }
       public List<QuickAction> QuickActions { get; set; }
       public KeyColorPicker KeyColorPicker { get; set; }
    }
}
