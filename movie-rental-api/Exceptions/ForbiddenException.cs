using System.Net;

namespace movie_rental_api.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message, string parameter) : base(message, parameter, HttpStatusCode.Forbidden)
        {
        }
    }
}