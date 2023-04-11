using movie_rental_api.Validator;
using Newtonsoft.Json;

namespace movie_rental_api.Models
{
    public class UpdateCustomerPhoneNumberModel : CustomerValidator
    {
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("telephone_number")]
        [TelephoneNumberValidate]
        public string TelephoneNumber { get; set; }
    }
}