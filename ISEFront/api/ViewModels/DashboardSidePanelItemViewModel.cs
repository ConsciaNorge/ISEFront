using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.api.ViewModels
{
    public class DashboardSidePanelItemViewModel
    {
        [JsonProperty("url")]
        public string ActionUrl { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}