using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMart.Models
{
    public class Camera
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }   //Comment

        [JsonProperty("direction", NullValueHandling = NullValueHandling.Ignore)]
        public string direction { get; set; }   //Comment

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }   //Comment

        [JsonProperty("factoryId", NullValueHandling = NullValueHandling.Ignore)]
        public string factoryId { get; set; }   //Comment


    }
}
