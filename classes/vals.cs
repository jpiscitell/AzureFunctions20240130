using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AZUREFUNCTIONS20240130.classes {
    public class somevals {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("process", NullValueHandling = NullValueHandling.Ignore)]
        public string process { get; set; }
        [JsonProperty("dateCreated", NullValueHandling = NullValueHandling.Ignore)]
        public string dateCreated { get; set; }
        [JsonProperty("minutes", NullValueHandling = NullValueHandling.Ignore)]
        public int minutes { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }
        [JsonProperty("subvals", NullValueHandling = NullValueHandling.Ignore)]
        public somesubvals[] subvals {get; set;}

        [JsonProperty("sftpcreds", NullValueHandling = NullValueHandling.Ignore)]
        public somesftpcreds[] sftpcreds {get; set;}
    }

    public class somesubvals {
        [JsonProperty("subval1", NullValueHandling = NullValueHandling.Ignore)]
        public int subval1 { get; set; }
        [JsonProperty("subval2", NullValueHandling = NullValueHandling.Ignore)]
        public string subval2 { get; set; }
    }

    public class somesftpcreds {
        [JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        public string host { get; set; }
        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public int port { get; set; }
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string username { get; set; }
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string password { get; set; }
    }
}