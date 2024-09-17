namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The ServiceErrorException class describes an error occurring when
    /// the API encounters an unexpected issue.
    /// </summary>
    public class ServiceErrorException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceErrorException(string message = "An unexpected error has occurred.")
            : base(500, message)
        {

        }
    }
}
