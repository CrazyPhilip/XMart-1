using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMart.Models
{
    public class RegisterByOpenIdPara : RegisterPara
    {
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string city { get; set; }   //Comment

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }   //Comment

        [JsonProperty("headimgurl", NullValueHandling = NullValueHandling.Ignore)]
        public string headimgurl { get; set; }   //Comment

        [JsonProperty("nikename", NullValueHandling = NullValueHandling.Ignore)]
        public string nikename { get; set; }   //Comment

        [JsonProperty("openId", NullValueHandling = NullValueHandling.Ignore)]
        public string openId { get; set; }   //Comment

        [JsonProperty("province", NullValueHandling = NullValueHandling.Ignore)]
        public string province { get; set; }   //Comment

    }
}
