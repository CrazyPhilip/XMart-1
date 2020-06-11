using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

using XMart.Models;

namespace XMart.ResponseData
{
    public class RebateRD : CommonRD
    {
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public List<RebateInfo> result { get; set; }   //comment
    }
}
