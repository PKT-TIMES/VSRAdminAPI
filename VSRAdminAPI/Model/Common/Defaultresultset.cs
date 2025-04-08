using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model.Common
{
    public class Defaultresultset
    {
        [JsonPropertyName("status")]
        public Int32 Status { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
