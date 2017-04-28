using Newtonsoft.Json;

namespace CiscoISE.Models
{
    public class GuestUserFieldViewModel
    {
        [JsonProperty("customType")]
        public bool CustomType { get; set; }
        [JsonProperty("dataType")]
        public string DataType { get; set; }
        [JsonProperty("dictionaryLabelKey")]
        public string DictionaryLabelKey { get; set; }
        [JsonProperty("labelName")]
        public string LabelName { get; set; }
        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}