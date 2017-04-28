using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace libciscoise
{
    [JsonObject("GuestUser")]
    public class GuestUserViewModel : GuestUserBriefViewModel
    {
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [JsonProperty("sponsorUserName", NullValueHandling = NullValueHandling.Ignore)]
        public string SponsorUsername { get; set; }
        [JsonProperty("sponsorUserId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid SponsorUserId { get; set; }
        [JsonProperty("guestInfo", NullValueHandling = NullValueHandling.Ignore)]
        public GuestInfoViewModel GuestInfo { get; set; }
        [JsonProperty("guestAccessInfo", NullValueHandling = NullValueHandling.Ignore)]
        public GuestAccessInfoViewModel GuestAccessInfo { get; set; }
        [JsonProperty("customFields", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, object> CustomFields { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("guestType", NullValueHandling = NullValueHandling.Ignore)]
        public string GuestType { get; set; }
        [JsonProperty("reasonForVisit", NullValueHandling = NullValueHandling.Ignore)]
        public string ReasonForVisit { get; set; }
        [JsonProperty("statusReason", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusReason { get; set; }
        [JsonProperty("portalId", NullValueHandling = NullValueHandling.Ignore)]
        public string PortalId { get; set; }
    }
}

