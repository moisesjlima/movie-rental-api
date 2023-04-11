using movie_rental_api.Validator;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace movie_rental_api.Models
{
    public class CreateCustomerModel : CustomerValidator
    {
        [JsonProperty("name")]
        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Nome usuário deve conter entre 1 a 100 caracteres.")]
        public string Name { get; set; }

        [JsonProperty("cpf")]
        [CPFValidate]
        public string CPF { get; set; }

        [JsonProperty("birth_date")]
        [Required(ErrorMessage = "Necessário informar data de nascimento")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("email")]
        [EmailValidate]
        [Required]
        public string Email { get; set; }

        [JsonProperty("telephone_number")]
        [TelephoneNumberValidate]
        public string TelephoneNumber { get; set; }
    }

}