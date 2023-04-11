using System.Net;

namespace movie_rental_api.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message, string parameter) : base(message, parameter, HttpStatusCode.Conflict)
        {
        }
    }
}
