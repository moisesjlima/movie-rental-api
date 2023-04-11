using Microsoft.AspNetCore.Mvc;
using movie_rental_api.Context;
using movie_rental_api.Models;
using Newtonsoft.Json;

namespace movie_rental_api.Controllers
{
    [Route("v1/rental-movie")]
    [ApiController]
    public class RentalMovieController : ControllerBase
    {
        private string API_KEY = Environment.GetEnvironmentVariable("API_KEY");
        private readonly MovieRentalContext _rentalContext;
        private readonly HttpClient _httpClient;

        public RentalMovieController(MovieRentalContext rentalContext)
        {
            _rentalContext = rentalContext;
            _httpClient = new HttpClient();
        }

        [HttpGet("Omdb/{movieName}")]
        public async Task<ActionResult> GetOmdbMoviesByName(string movieName)
        {
            var request = await _httpClient.GetAsync($"https://www.omdbapi.com/?apikey={API_KEY}&type=movie&s={movieName}");
            var jsonString = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<OmdbListModel>(jsonString);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetRentalMovies()
        {
            var response = _rentalContext.RentalMovie.ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRentalMovie(CreateRentalMovieModel createRentalMovieModel)
        {
            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == createRentalMovieModel.CustomerId);

            if (customer == null)
                return BadRequest("Cliente não encontrado, cadastre o cliente para alugar o filme");

            var customerAge = DateTime.UtcNow.Year - customer.BirthDate.Year;

            if (customerAge < 14)
                return StatusCode(StatusCodes.Status403Forbidden, "cliente menor de 14 anos de idade não permitido");

            var rentalMovie = _rentalContext.RentalMovie.FirstOrDefault(x => x.ImdbId == createRentalMovieModel.ImdbId);

            if (rentalMovie != null)
                return BadRequest("Filme Já alugado");

            var customerList = _rentalContext.RentalMovie.Where(x => x.CustomerId == createRentalMovieModel.CustomerId).ToList();

            if (customerList.Count >= 2)
                return StatusCode(StatusCodes.Status403Forbidden, "cliente já possui 2 filmes alugados");

            var model = new RentalMovie
            {
                ImdbId = createRentalMovieModel.ImdbId,
                CustomerId = createRentalMovieModel.CustomerId,
                RentalStartDate = createRentalMovieModel.RentalStartDate,
                RentalEndDate = createRentalMovieModel.RentalEndDate
            };

            _rentalContext.Add(model);
            _rentalContext.SaveChanges();

            return Created($"v1/rental-movie/{model.RentalMovieId}", model);
        }

        [HttpDelete("{rentalMovieId:int}")]
        public async Task<ActionResult> DeleteRentalMovie(int rentalMovieId)
        {
            var rentalMovie = _rentalContext.RentalMovie.FirstOrDefault(x => x.RentalMovieId == rentalMovieId);

            if (rentalMovie == null)
                return NotFound();

            _rentalContext.RentalMovie.Remove(rentalMovie);
            _rentalContext.SaveChanges();

            return NoContent();
        }
    }
}