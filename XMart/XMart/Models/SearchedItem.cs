using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMart.Models
{
    public class SearchedItem
    {
        [JsonProperty("createTime", NullValueHandling = NullValueHandling.Ignore)]
        public string createTime { get; set; }   //Comment

        [JsonProperty("searchedString", NullValueHandling = NullValueHandling.Ignore)]
        public string searchedString { get; set; }   //Comment


    }
}
