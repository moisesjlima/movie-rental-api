using System.Net;

namespace movie_rental_api.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message, string parameter) : base(message, parameter, HttpStatusCode.NotFound)
        {
        }
    }
}
