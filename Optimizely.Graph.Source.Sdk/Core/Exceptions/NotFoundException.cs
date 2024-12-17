namespace Optimizely.Graph.Source.Sdk.Core.Exceptions
{
    /// <summary>
    /// The NotFoundException class describes an error occurring when
    /// the API cannot find a requested item.
    /// </summary>
    public class NotFoundException : ClientException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public NotFoundException(string message = "The requested item could not be found.")
            : base(404, message)
        {

        }
    }
}
