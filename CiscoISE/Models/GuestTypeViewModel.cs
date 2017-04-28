using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class GuestTypeViewModel : GuestTypeBriefViewModel
    {
        [JsonProperty("accessTime")]
        public GuestTypeAccessTimeViewModel AccessTime { get; set; }
        [JsonProperty("loginOptions")]
        public LoginOptionsViewModel LoginOptions { get; set; }
        [JsonProperty("expirationNotification")]
        public ExpirationNotificationViewModel ExpirationNotification { get; set; }
        [JsonProperty("sponsorGroups")]
        public List<string> SponsorGroups { get; set; }
    }
}
