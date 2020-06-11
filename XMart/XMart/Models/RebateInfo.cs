using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace XMart.Models
{
    public class RebateInfo
    {
        [JsonProperty("buyerId", NullValueHandling = NullValueHandling.Ignore)]
        public long buyerId { get; set; }   //Comment

        [JsonProperty("buyerName", NullValueHandling = NullValueHandling.Ignore)]
        public string buyerName { get; set; }   //Comment

        [JsonProperty("buyerPhone", NullValueHandling = NullValueHandling.Ignore)]
        public string buyerPhone { get; set; }   //Comment

        [JsonProperty("buyerSex", NullValueHandling = NullValueHandling.Ignore)]
        public string buyerSex { get; set; }   //Comment

        [JsonProperty("createDate", NullValueHandling = NullValueHandling.Ignore)]
        public string createDate { get; set; }   //Comment

        [JsonProperty("productId", NullValueHandling = NullValueHandling.Ignore)]
        public long productId { get; set; }   //Comment

        [JsonProperty("productName", NullValueHandling = NullValueHandling.Ignore)]
        public string productName { get; set; }   //Comment

        [JsonProperty("rebate", NullValueHandling = NullValueHandling.Ignore)]
        public double rebate { get; set; }   //Comment


    }
}
