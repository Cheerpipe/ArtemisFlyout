using Newtonsoft.Json;

namespace ArtemisFlyout.Models
{

    //TODO: Create models
    public class Category
    {
        [JsonProperty("$id")]
        public string Iid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsSuspended { get; set; }
    }

    public class Profile
    {
        [JsonProperty("$id")]
        public string Iid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsBeingEdited { get; set; }
        public bool IsMissingModule { get; set; }
        public bool HasActivationCondition { get; set; }
        public bool ActivationConditionMet { get; set; }
        public Category Category { get; set; }
    }
}