
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClearsaleCadastro
{
    public class SendDataAccountRequest
    {

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "sessionID")]
        public string SessionId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "personDocument")]
        public string PersonDocument { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "companyDocument")]
        public string CompanyDocument { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "passwordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "optinEmail")]
        public string OptinEmail { get; set; }

        [JsonProperty(PropertyName = "optinMobile")]
        public string OptinMobile { get; set; }

        [JsonProperty(PropertyName = "addresses")]
        public List<SendDataAccountAddress> Addresses { get; set; }

        [JsonProperty(PropertyName = "phones")]
        public List<SendDataAccountPhones> Phones { get; set; }
    }

    public class SendDataAccountPhones
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ddi")]
        public string Ddi { get; set; }

        [JsonProperty(PropertyName = "ddd")]
        public string Ddd { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "extension")]
        public string Extension { get; set; }

    }

    public class SendDataAccountAddress
    {

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "comp")]
        public string Complement { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "zipcode")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }
    }
}