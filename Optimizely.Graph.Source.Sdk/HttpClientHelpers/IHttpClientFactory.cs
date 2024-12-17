namespace Optimizely.Graph.Source.Sdk.HttpClientHelpers
{
    /// <summary>
    /// The IHttpClientFactory interface describes a factory capable of constructing instances of HttpClient.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates an instance of HttpClient.
        /// </summary>
        /// <param name="serviceBaseAddress">Base address for target service.</param>
        /// <returns>A new instance of HttpClient.</returns>
        HttpClient Create(Uri serviceBaseAddress);
    }
}
