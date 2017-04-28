using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class ExpirationNotificationViewModel
    {
        [JsonProperty("enableNotification")]
        public bool EnableNotification { get; set; }
        [JsonProperty("advanceNotificationDuration")]
        public int AdvanceNotificationNotification { get; set; }
        [JsonProperty("advanceNotificationUnits")]
        public string AdvanceNotificationUnits { get; set; }
        [JsonProperty("sendEmailNotification")]
        public bool SendEmailNotification { get; set; }
        [JsonProperty("emailText")]
        public string EmailText { get; set; }
        [JsonProperty("sendSmsNotification")]
        public bool SendSmsNotification { get; set; }
        [JsonProperty("smsText")]
        public string SmsText { get; set; }
    }
}
