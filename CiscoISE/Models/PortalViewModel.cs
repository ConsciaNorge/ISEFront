using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class PortalViewModel : PortalBriefViewModel
    {
        [JsonProperty("allowSponsorToChangeOwnPassword")]
        public bool AllowSponsorToChangeOwnPassword { get; set; }
        [JsonProperty("guestUserFieldList")]
        public List<GuestUserFieldViewModel> GuestUserFieldList { get; set; }
        [JsonProperty("portalType")]
        public string PortalType { get; set; }
    }
}
