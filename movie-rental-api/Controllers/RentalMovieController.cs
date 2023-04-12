using Microsoft.AspNetCore.Mvc;
using movie_rental_api.Exceptions;
using movie_rental_api.Models;
using movie_rental_api.Services;

namespace movie_rental_api.Controllers
{
    [Route("v1/rental-movie")]
    [ApiController]
    public class RentalMovieController : ControllerBase
    {
        private readonly MovieRentalService _movieRentalService;

        public RentalMovieController(MovieRentalService movieRentalService)
        {
            _movieRentalService = movieRentalService;
        }

        [HttpGet("Omdb/{movieName}")]
        public async Task<ActionResult> GetOmdbMoviesByName(string movieName)
        {
            var response = await _movieRentalService.GetOmdbMoviesByName(movieName);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetRentalMovies()
        {
            var response = _movieRentalService.GetRentalMovies();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRentalMovie(CreateRentalMovieModel createRentalMovieModel)
        {
            try
            {
                var rentalMovie = _movieRentalService.CreateRentalMovie(createRentalMovieModel);

                return Created($"v1/rental-movie/{rentalMovie.RentalMovieId}", rentalMovie);
            }
            catch (ForbiddenException e)
            {
                return BadRequest(new ForbiddenException(e.Message, e.Parameter));
            }
            catch (BadRequestException e)
            {
                return BadRequest(new BadRequestException(e.Message, e.Parameter));
            }
        }

        [HttpDelete("{rentalMovieId:int}")]
        public async Task<ActionResult> DeleteRentalMovie(int rentalMovieId)
        {
            _movieRentalService.DeleteRentalMovie(rentalMovieId);

            return NoContent();
        }
    }
}