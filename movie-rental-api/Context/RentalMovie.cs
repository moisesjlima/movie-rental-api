using Newtonsoft.Json;

namespace movie_rental_api.Context
{
    public class RentalMovie
    {
        [JsonProperty("rental_movie_id")]
        public int RentalMovieId { get; set; }

        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("rental_start_date")]
        public DateTime RentalStartDate { get; set; }

        [JsonProperty("rental_end_date")]
        public DateTime RentalEndDate { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Customer Customer { get; set; }
    }
}