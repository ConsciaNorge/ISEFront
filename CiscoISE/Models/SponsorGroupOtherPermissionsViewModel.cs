using Newtonsoft.Json;

namespace CiscoISE.Models
{
    public class SponsorGroupOtherPermissionsViewModel
    {
        [JsonProperty("canUpdateGuestContactInfo")]
        public bool CanUpdateGuestContactInfo { get; set; }
        [JsonProperty("canViewGuestPasswords")]
        public bool CanViewGuestPasswords { get; set; }
        [JsonProperty("canSendSmsNotifications")]
        public bool CanSendSmsNotifications { get; set; }
        [JsonProperty("canResetGuestPasswords")]
        public bool CanResetGuestPasswords { get; set; }
        [JsonProperty("canExtendGuestAccounts")]
        public bool CanExtendGuestAccounts { get; set; }
        [JsonProperty("canDeleteGuestAccounts")]
        public bool CanDeleteGuestAccounts { get; set; }
        [JsonProperty("canSuspendGuestAccounts")]
        public bool CanSuspendGuestAccounts { get; set; }
        [JsonProperty("requireSuspensionReason")]
        public bool RequireSuspensionReason { get; set; }
        [JsonProperty("canReinstateSuspendedAccounts")]
        public bool CanReinstateSuspendedAccounts { get; set; }
        [JsonProperty("canApproveSelfregGuests")]
        public bool CanApproveSelfregGuests { get; set; }
        [JsonProperty("limitApprovalToSponsorsGuests")]
        public bool LimitApprovalToSponsorsGuests { get; set; }
        [JsonProperty("canAccessViaRest")]
        public bool CanAccessViaRest { get; set; }
    }
}
