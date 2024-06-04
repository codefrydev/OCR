using System.Net.Http.Headers;

public class CoreError(string message, int code, HttpResponseHeaders headers, string body) : Exception(message)
{
}
