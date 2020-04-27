using Newtonsoft.Json;

namespace XMart.Models
{
    public class UpdateUserPara
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }   //comment

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string username { get; set; }   //comment

        [JsonProperty("userType", NullValueHandling = NullValueHandling.Ignore)]
        public string userType { get; set; }   //comment

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }   //comment

        [JsonProperty("balance", NullValueHandling = NullValueHandling.Ignore)]
        public int balance { get; set; }   //comment

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }   //comment

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }   //comment

        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public string file { get; set; }   //comment

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string phone { get; set; }   //comment

        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public int points { get; set; }   //comment

        [JsonProperty("sex", NullValueHandling = NullValueHandling.Ignore)]
        public string sex { get; set; }   //comment

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public int state { get; set; }   //comment

        [JsonProperty("rebateTotal", NullValueHandling = NullValueHandling.Ignore)]
        public long rebateTotal { get; set; }   //comment

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public long created { get; set; }   //Comment

        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public long updated { get; set; }   //Comment

        [JsonProperty("personName", NullValueHandling = NullValueHandling.Ignore)]
        public string personName { get; set; }   //Comment

        [JsonProperty("pricture", NullValueHandling = NullValueHandling.Ignore)]
        public string pricture { get; set; }   //Comment

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }   //Comment

        [JsonProperty("companyProvince", NullValueHandling = NullValueHandling.Ignore)]
        public string companyProvince { get; set; }   //Comment

        [JsonProperty("companyCity", NullValueHandling = NullValueHandling.Ignore)]
        public string companyCity { get; set; }   //Comment

        [JsonProperty("companyName", NullValueHandling = NullValueHandling.Ignore)]
        public string companyName { get; set; }   //Comment

        [JsonProperty("invitePhone", NullValueHandling = NullValueHandling.Ignore)]
        public string invitePhone { get; set; }   //Comment

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string password { get; set; }   //Comment
    }
}
