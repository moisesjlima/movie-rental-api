using System.Net;

namespace movie_rental_api.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message, string parameter) : base(message, parameter, HttpStatusCode.BadRequest)
        {
        }
    }
}