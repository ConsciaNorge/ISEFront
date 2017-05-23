using ISEFront.api.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.Utility.Configuration
{
    public class SettingsViewModel
    {
        [JsonProperty("iseServerSettings")]
        public ISEServerSettingsViewModel IseServerSettings { get; set; }

        [JsonProperty("bankIDSettings")]
        public BankIDSettingsViewModel BankIDSettings { get; set; }
    }
}