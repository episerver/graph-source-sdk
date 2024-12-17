namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The ClientException class describes an error occurring while
    /// interacting with a client.
    /// </summary>
    public class ClientException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        public ClientException(int statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Message describing the error</param>
        public ClientException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="inner">Inner exception</param>
        public ClientException(string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = 0;
        }

        /// <summary>
        /// Gets or sets the HTTP status code corresponding to the error.
        /// </summary>
        public int StatusCode { get; private set; }
    }
}
