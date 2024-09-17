namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The CommunicationException class describes an error occurring in the
    /// course of communicating with the API, such as a connection
    /// timeout or interruption.
    /// </summary>
    public class CommunicationException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="inner">Inner exception</param>
        public CommunicationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
