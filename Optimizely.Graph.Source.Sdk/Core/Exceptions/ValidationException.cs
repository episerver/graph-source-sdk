namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The ValidationException class describes an error occurring when
    /// the API receives a request with invalid parameters.
    /// </summary>
    public class ValidationException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ValidationException(string message) : base(400, message)
        {
        }
    }
}
