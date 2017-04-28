using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class LoginOptionsViewModel
    {
        [JsonProperty("limitSimultaneousLogins")]
        public bool LimitSimultaneousLogins { get; set; }
        [JsonProperty("maxSimultaneousLogins")]
        public int MaxSimultaneousLogins { get; set; }
        [JsonProperty("failureAction")]
        public string FailureAction { get; set; }
        [JsonProperty("maxRegisteredDevices")]
        public int MaxRegisteredDevices { get; set; }
        [JsonProperty("identityGroupId")]
        public string IdentityGroupId { get; set; }
        [JsonProperty("allowGuestPortalBypass")]
        public bool AllowGuestPortalBypass { get; set; }
    }
}
