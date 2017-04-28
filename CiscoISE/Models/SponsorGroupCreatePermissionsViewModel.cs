using Newtonsoft.Json;

namespace CiscoISE.Models
{
    public class SponsorGroupCreatePermissionsViewModel
    {
        [JsonProperty("canImportMultipleAccounts")]
        public bool CanImportMultipleAccounts { get; set; }
        [JsonProperty("importBatchSizeLimit")]
        public int ImportBatchSizeLimit { get; set; }
        [JsonProperty("canCreateRandomAccounts")]
        public bool CanCreateRandomAccounts { get; set; }
        [JsonProperty("RandomBatchSizeLimit")]
        public int RandomBatchSizeLimit { get; set; }
        [JsonProperty("defaultUsernamePrefix")]
        public string DefaultUsernamePrefix { get; set; }
        [JsonProperty("canSpecifyUsernamePrefix")]
        public bool CanSpecifyUsernamePrefix { get; set; }
        [JsonProperty("canSetFutureStartDate")]
        public bool CanSetFutureStartDate { get; set; }
        [JsonProperty("startDateFutureLimitDays")]
        public int StartDateFutureLimitDays { get; set; }
    }
}
