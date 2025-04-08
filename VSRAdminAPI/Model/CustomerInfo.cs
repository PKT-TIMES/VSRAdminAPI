using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class CustomerInfo
    {
        [JsonPropertyName("companyid")]
        public Int32 Companyid { get; set; }
        [JsonPropertyName("merchantid")]
        public string? Merchantid { get; set; }
        [JsonPropertyName("clientid")]
        public string? Clientid { get; set; }
        [JsonPropertyName("secretkey")]
        public string? Secretkey { get; set; }
        [JsonPropertyName("secretcode")]
        public string? Secretcode { get; set; }
        [JsonPropertyName("authtoken")]
        public string? Authtoken { get; set; }
        [JsonPropertyName("pos")]
        public string? Pos { get; set; }
    }
}
