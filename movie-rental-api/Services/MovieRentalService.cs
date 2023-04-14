using movie_rental_api.Context;
using movie_rental_api.Enums;
using movie_rental_api.Exceptions;
using movie_rental_api.Models;
using Newtonsoft.Json;

namespace movie_rental_api.Services
{
    public class MovieRentalService
    {
        readonly string API_KEY = Environment.GetEnvironmentVariable("API_KEY");
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
            if (!response.Response)
            {
                throw new NotFoundException("nenhum filme encontrado na lista", "rentalMoive.no_movies_found");
            }

            return response;
        }

        public IEnumerable<RentalMovie> GetRentalMovies()
        {
            var rentalMovieList = _rentalContext.RentalMovie.ToList();

            return rentalMovieList;
        }

        public async Task<RentalMovie> CreateRentalMovieAsync(CreateRentalMovieModel createRentalMovieModel)
        {
            if (createRentalMovieModel.RentalStartDate >= createRentalMovieModel.RentalEndDate)
                throw new ForbiddenException("data inicial do aluguel não pode ser maior ou igual ao prazo de entrega", "rentalMovie.bad_request");

            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == createRentalMovieModel.CustomerId);

            if (customer == null)
                throw new BadRequestException("Cliente não encontrado, cadastre o cliente para alugar o filme", "rentalMovie.bad_request");

            var customerAgeAllowed = customer.BirthDate.AddYears(14) > DateTime.UtcNow;

            if (customerAgeAllowed)
                throw new ForbiddenException("cliente menor de 14 anos de idade não permitido", "customer_not_allowed");

            var rentalMovie = _rentalContext.RentalMovie.FirstOrDefault(x => x.ImdbId == createRentalMovieModel.ImdbId);

            if (rentalMovie != null && rentalMovie.Status != RentalMovieStatusEnum.FINISHED)
                throw new BadRequestException("Filme já alugado", "rentalMovie.movie_already_rented");

            await requestOmdb(createRentalMovieModel.ImdbId);

            var customerMovieList = _rentalContext.RentalMovie.Where(x => x.CustomerId == createRentalMovieModel.CustomerId && x.Status != RentalMovieStatusEnum.FINISHED).ToList();

            if (customerMovieList.Count >= 2)
                throw new ForbiddenException("cliente já possui 2 filmes alugados", "customer.has_max_movies_rented");

            var model = new RentalMovie
            {
                ImdbId = createRentalMovieModel.ImdbId,
                CustomerId = createRentalMovieModel.CustomerId,
                Status = RentalMovieStatusEnum.ACTIVE,
                RentalStartDate = createRentalMovieModel.RentalStartDate.Date,
                RentalEndDate = createRentalMovieModel.RentalEndDate.Date
            };

            _rentalContext.Add(model);
            _rentalContext.SaveChanges();

            return model;
        }

        public async Task requestOmdb(string imdbId)
        {
            var request = await _httpClient.GetAsync($"https://www.omdbapi.com/?apikey={API_KEY}&type=movie&i={imdbId}");
            var jsonString = await request.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<OmdbListModel>(jsonString);
            if (!response.Response)
                throw new BadRequestException("filme não encontrado pelo id para locação", "rentalMovie.not_found");
        }

        public void RemoveRentalMovie(int rentalMovieId)
        {
            var rentalMovie = _rentalContext.RentalMovie.FirstOrDefault(x => x.RentalMovieId == rentalMovieId);

            if (rentalMovie == null)
                throw new NotFoundException("aluguel não encontrado", "rentalMovie_notFound");

            if (rentalMovie.Status == RentalMovieStatusEnum.OVERDUE)
                throw new NotFoundException("aluguel se encontrado atrasado, não é possível encerrar a locação", "rentalMovie_cannot_be_removed");

            rentalMovie.Status = RentalMovieStatusEnum.FINISHED;
            _rentalContext.SaveChanges();
        }
    }
}