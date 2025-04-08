using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class CompanyInfo
    {
        [JsonPropertyName("sno")]
        public Int32 Sno { get; set; }
        [JsonPropertyName("customerid")]
        public Int32 Customerid { get; set; }
        [JsonPropertyName("customername")]
        public string? Customername { get; set; }
        [JsonPropertyName("companyname")]
        public string? Companyname { get; set; }
        [JsonPropertyName("regnumber")]
        public string? Regnumber { get; set; }
        [JsonPropertyName("address1")]
        public string? Address1 { get; set; }
        [JsonPropertyName("address2")]
        public string? Address2 { get; set; }
        [JsonPropertyName("address3")]
        public string? Address3 { get; set; }
        [JsonPropertyName("country")]
        public string? Country { get; set; }
        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }
        [JsonPropertyName("contactnumber")]
        public string? Contactnumber { get; set; }
        [JsonPropertyName("contactperson")]
        public string? Contactperson { get; set; }
        [JsonPropertyName("contactpernum")]
        public string? Contactpernum { get; set; }
        [JsonPropertyName("did")]
        public string? Did { get; set; }
        [JsonPropertyName("onlineurl")]
        public string? Onlineurl { get; set; }
        [JsonPropertyName("delivertype")]
        public Int32 Delivertype { get; set; }
        [JsonPropertyName("ordercomission")]
        public decimal Ordercomission { get; set; }
        [JsonPropertyName("hours")]
        public string? Hours { get; set; }
        [JsonPropertyName("minutes")]
        public string? Minutes { get; set; }
        [JsonPropertyName("queuename")]
        public string? Queuename { get; set; }
        [JsonPropertyName("whatsappurl")]
        public string? Whatsappurl { get; set; }
        [JsonPropertyName("whatsappinstructions")]
        public string? Whatsappinstructions { get; set; }
        [JsonPropertyName("imagepath")]
        public string? Imagepath { get; set; }
        [JsonPropertyName("isadmin")]
        public int? Isadmin { get; set; }
        [JsonPropertyName("tax")]
        public decimal? Tax { get; set; }
        [JsonPropertyName("taxname")]
        public string? Taxname { get; set; }
    }
}
