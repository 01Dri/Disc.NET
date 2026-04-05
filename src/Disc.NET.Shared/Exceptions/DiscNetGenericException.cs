namespace Disc.NET.Shared.Exceptions
{
    internal class DiscNetGenericException : Exception
    {
        public DiscNetGenericException()
        {
        }
        public DiscNetGenericException(string message)
            : base(message)
        {
        }
    }
}
