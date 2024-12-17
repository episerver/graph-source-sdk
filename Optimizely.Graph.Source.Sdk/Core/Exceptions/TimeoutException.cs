namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The TimeoutException class describes an error occurring when
    /// the request to the API times out.
    /// </summary>
    public class TimeoutException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public TimeoutException(string message = "The server did not send a request in the time provided by the client.")
            : base(408, message)
        {

        }
    }
}
