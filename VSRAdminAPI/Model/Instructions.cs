using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class ReqInput
    {
        [JsonPropertyName("customerid")]
        public Int32 Customerid { get; set; }
        [JsonPropertyName("instruction")]
        public string? Instruction { get; set; }
    }
    public class LoadInstructionRes
    {
        [JsonPropertyName("instruction")]
        public string? Instruction { get; set; }
    }
    public class OPRes
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
