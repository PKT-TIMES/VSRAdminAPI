using System.Text.Json.Serialization;

namespace VSRAdminAPI.Model
{
    public class MasterCustomer
    {
        [JsonPropertyName("type")]
        public Int32 Type { get; set; }
        [JsonPropertyName("companyname")]
        public string? CompanyName { get; set; }
        [JsonPropertyName("companyid")]
        public Int32 CompanyId { get; set; }
        [JsonPropertyName("customername")]
        public string? CustomerName { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("firstname")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string? LastName { get; set; }
        [JsonPropertyName("regnumber")]
        public string? RegNumber { get; set; }

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
        public string? ContactNumber { get; set; }

        [JsonPropertyName("contactperson")]
        public string? ContactPerson { get; set; }

        [JsonPropertyName("contactpersonnumber")]
        public string? ContactPersonNumber { get; set; }

        [JsonPropertyName("did")]
        public string? DID { get; set; }

        [JsonPropertyName("onlineurl")]
        public string? OnlineURL { get; set; }

        [JsonPropertyName("deliverytype")]
        public int DeliveryType { get; set; }

        [JsonPropertyName("ordercomission")]
        public decimal? OrderComission { get; set; }
        [JsonPropertyName("createdby")]
        public Int32 Createdby { get; set; }
        [JsonPropertyName("restauranttype")]
        public Int32 Restauranttype { get; set; }
        [JsonPropertyName("hours")]
        public string? Hours { get; set; }
        [JsonPropertyName("minutes")]
        public string? Minutes { get; set; }
        [JsonPropertyName("queuename")]
        public string? Queuename { get; set; }
        [JsonPropertyName("whatsappurl")]
        public string? WhatsappURL { get; set; }
        [JsonPropertyName("whatsappinstructions")]
        public string? WhatsappInstructions { get; set; }
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
