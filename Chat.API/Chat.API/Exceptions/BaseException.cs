using System.Net;

namespace Chat.API.Exceptions;

public class BaseException(string message, HttpStatusCode statusCode) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}