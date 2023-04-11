using Newtonsoft.Json;

namespace movie_rental_api.Models
{
    public class CreateRentalMovieModel
    {
        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("rental_start_date")]
        public DateTime RentalStartDate { get; set; }

        [JsonProperty("rental_end_date")]
        public DateTime RentalEndDate { get; set; }
    }
}
