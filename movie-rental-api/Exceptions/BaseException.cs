using System.Net;

namespace movie_rental_api.Exceptions
{
    public class BaseException : Exception
    {
        public string Message { get; set; }
        public string Parameter { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public BaseException(string message, string parameter, HttpStatusCode statusCode)
        {
            Message = message;
            Parameter = parameter;
            StatusCode = statusCode;
        }
    }
}