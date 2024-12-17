namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The ForbiddenException class describes an error occurring when
    /// the API cannot complete the request.
    /// </summary>
    public class ForbiddenException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public ForbiddenException(string message = "The request is forbidden.")
            : base(403, message)
        {

        }
    }
}
