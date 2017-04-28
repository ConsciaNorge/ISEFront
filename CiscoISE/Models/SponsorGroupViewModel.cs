using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class SponsorGroupViewModel : SponsorGroupBriefViewModel
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }
        [JsonProperty("isDefaultGroup")]
        public bool IsDefaultGroup { get; set; }
        [JsonProperty("memberGroups")]
        public List<string> MemberGroups { get; set; }
        [JsonProperty("guestTypes")]
        public List<string> GuestTypes { get; set; }
        [JsonProperty("locations")]
        public List<string> Locations { get; set; }
        [JsonProperty("autoNotification")]
        public bool AutoNotification { get; set; }
        [JsonProperty("createPermissions")]
        public SponsorGroupCreatePermissionsViewModel CreatePermissions { get; set; }
        [JsonProperty("managePermissions")]
        public string ManagePermissions { get; set; }
        [JsonProperty("otherPermissions")]
        public SponsorGroupOtherPermissionsViewModel OtherPermissions { get; set; }
    }
}
