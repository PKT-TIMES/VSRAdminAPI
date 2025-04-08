
using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class CustomerFileData
    {
        [JsonPropertyName("customerdata")]
        public string? Customerdata { get; set; }
        [JsonPropertyName("filename")]
        public IFormFile? Filename { get; set; }

    }
}
