using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class LoginResultset
    {
        [JsonPropertyName("userid")]
        public int Userid { get; set; }
        [JsonPropertyName("password")]
        public int Password { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }
        [JsonPropertyName("emalid")]
        public string? Emalid { get; set; }
        [JsonPropertyName("company")]
        public string? Company { get; set; }
        [JsonPropertyName("companyid")]
        public int Companyid { get; set; }
        [JsonPropertyName("roleid")]
        public int Roleid { get; set; }
        [JsonPropertyName("rolename")]
        public string? Rolename { get; set; }
        [JsonPropertyName("lastlogin")]
        public string? Lastlogin { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("approvetype")]
        public int Approvetype { get; set; }
        [JsonPropertyName("issuperadmin")]
        public int Issuperadmin { get; set; }
        [JsonPropertyName("isadmin")]
        public int Isadmin { get; set; }
        [JsonPropertyName("hasapprove")]
        public int Hasapprove { get; set; }
    }
    public class LoginValues
    {
        [JsonPropertyName("username")]
        public string? UserName { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
