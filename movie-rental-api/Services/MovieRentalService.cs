using movie_rental_api.Context;
using movie_rental_api.Exceptions;
using movie_rental_api.Models;
using Newtonsoft.Json;

namespace movie_rental_api.Services
{
    public class MovieRentalService
    {
        private string API_KEY = Environment.GetEnvironmentVariable("API_KEY");
        private readonly MovieRentalContext _rentalContext;
        private readonly HttpClient _httpClient;

        public MovieRentalService(MovieRentalContext rentalContext)
        {
            _rentalContext = rentalContext;
            _httpClient = new HttpClient();
        }

        public async Task<OmdbListModel> GetOmdbMoviesByName(string movieName)
        {
            var request = await _httpClient.GetAsync($"https://www.omdbapi.com/?apikey={API_KEY}&type=movie&s={movieName}");
            var jsonString = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<OmdbListModel>(jsonString);

            return response;
        }

        public IEnumerable<RentalMovie> GetRentalMovies()
        {
            var rentalMovieList = _rentalContext.RentalMovie.ToList();

            return rentalMovieList;
        }

        public RentalMovie CreateRentalMovie(CreateRentalMovieModel createRentalMovieModel)
        {
            if (createRentalMovieModel.RentalStartDate >= createRentalMovieModel.RentalEndDate)
                throw new ForbiddenException("data inicial do aluguel não pode ser menor ou igual ao prazo de entrega", "rentalMovie.bad_request");

            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == createRentalMovieModel.CustomerId);

            if (customer == null)
                throw new BadRequestException("Cliente não encontrado, cadastre o cliente para alugar o filme", "rentalMovie.bad_request");

            var customerAge = DateTime.UtcNow.Year - customer.BirthDate.Year;

            if (customerAge < 14)
                throw new ForbiddenException("cliente menor de 14 anos de idade não permitido", "customer_not_allowed");

            var rentalMovie = _rentalContext.RentalMovie.FirstOrDefault(x => x.ImdbId == createRentalMovieModel.ImdbId);

            if (rentalMovie != null)
                throw new BadRequestException("Filme já alugado", "rentalMovie.movie_already_rented");

            var customerList = _rentalContext.RentalMovie.Where(x => x.CustomerId == createRentalMovieModel.CustomerId).ToList();

            if (customerList.Count >= 2)
                throw new ForbiddenException("cliente já possui 2 filmes alugados", "customer.has_max_movies_rented");

            var model = new RentalMovie
            {
                ImdbId = createRentalMovieModel.ImdbId,
                CustomerId = createRentalMovieModel.CustomerId,
                RentalStartDate = createRentalMovieModel.RentalStartDate,
                RentalEndDate = createRentalMovieModel.RentalEndDate
            };

            _rentalContext.Add(model);
            _rentalContext.SaveChanges();

            return model;
        }

        public void DeleteRentalMovie(int rentalMovieId)
        {
            var rentalMovie = _rentalContext.RentalMovie.FirstOrDefault(x => x.RentalMovieId == rentalMovieId);

            if (rentalMovie == null)
                throw new NotFoundException("Não foi encontrado o aluguel para exclusão", "rentalMovie.not_found");

            _rentalContext.RentalMovie.Remove(rentalMovie);
            _rentalContext.SaveChanges();
        }
    }
}