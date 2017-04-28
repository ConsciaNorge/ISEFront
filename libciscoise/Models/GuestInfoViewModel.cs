using Newtonsoft.Json;
using System;

namespace libciscoise
{
    [JsonObject("GuestInfo")]
    public class GuestInfoViewModel
    {
        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }
        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("emailAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string EmailAddress { get; set; }
        [JsonProperty("password"]
        public string Password { get; set; }
        [JsonProperty("creationTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset CreationTime { get; set; }
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        [JsonProperty("notificationLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public string NotificationLanguage { get; set; }
        [JsonProperty("smsServiceProvider", NullValueHandling = NullValueHandling.Ignore)]
        public string SMSProvider { get; set; }
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public string Company { get; set; }
    }
}