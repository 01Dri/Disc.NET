using System.Net;

namespace Disc.NET.Shared.Exceptions
{
    internal class DiscNetClientSdkException : Exception
    {
        public DiscNetClientSdkException()
        {
        }
        public DiscNetClientSdkException(string message, HttpStatusCode statusCode)
            : base($"Discord API request failed with status code: {(int)statusCode} \n Error message: {message}")
        {
        }
        public DiscNetClientSdkException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
