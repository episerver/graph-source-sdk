namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The NotAuthorizedException class describes an error occurring when
    /// the client fails to authenticate with the API.
    /// </summary>
    public class NotAuthorizedException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public NotAuthorizedException(string message = "This client is not authorized.")
            : base(401, message)
        {

        }
    }
}
