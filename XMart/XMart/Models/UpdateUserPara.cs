using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMart.Models
{
    public class UpdateUserPara
    {
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string city { get; set; }   //Comment

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }   //Comment

        [JsonProperty("district", NullValueHandling = NullValueHandling.Ignore)]
        public string district { get; set; }   //Comment

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }   //Comment

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }   //Comment

        [JsonProperty("memberfile", NullValueHandling = NullValueHandling.Ignore)]
        public string memberfile { get; set; }   //Comment

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string password { get; set; }   //Comment

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string phone { get; set; }   //Comment

        [JsonProperty("province", NullValueHandling = NullValueHandling.Ignore)]
        public string province { get; set; }   //Comment

        [JsonProperty("sex", NullValueHandling = NullValueHandling.Ignore)]
        public string sex { get; set; }   //Comment

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public int state { get; set; }   //Comment

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string username { get; set; }   //Comment
    }
}
