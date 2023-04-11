using Newtonsoft.Json;

namespace movie_rental_api.Context
{
    public class Customer
    {
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cpf")]
        public string CPF { get; set; }

        [JsonProperty("birth_date")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("telephone_number")]
        public string TelephoneNumber { get; set; }
    }
}