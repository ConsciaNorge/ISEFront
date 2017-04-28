using Newtonsoft.Json;
using System;

namespace libciscoise
{
    public class LinkViewModel
    {
        [JsonProperty("rel")]
        public string Rel { get; set; }
        [JsonProperty("href")]
        public Uri Href { get; set; }
        [JsonProperty("type")]
        public string MimeType { get; set; }
    }
}