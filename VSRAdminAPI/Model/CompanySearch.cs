using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class CompanySearch
    {
        [JsonPropertyName("search")]
        public string? Search { get; set; }
        [JsonPropertyName("pageno")]
        public Int32 Pageno { get; set; }
    }
}
