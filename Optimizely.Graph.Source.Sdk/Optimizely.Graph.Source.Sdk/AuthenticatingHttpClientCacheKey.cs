
namespace Optimizely.Graph.Source.Sdk
{
    /// <summary>
    /// The AuthenticatingHttpClientCacheKey class represents a unique key applied in the
    /// caching of HttpClient instances which bear credentials for authenticating against
    /// target services.
    /// </summary>
    public class AuthenticatingHttpClientCacheKey : HttpClientCacheKey
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint">Base address targeted by the HttpClient</param>
        /// <param name="applicationKey">Application key identifying the tenant</param>
        /// <param name="secretKey">Secret key for the tenant</param>
        public AuthenticatingHttpClientCacheKey(Uri endpoint, string applicationKey, string secretKey)
            : base(KeyFor(endpoint, applicationKey, secretKey))
        {

        }

        /// <summary>
        /// Produces a value representing a unique key for the specified HTTP client attributes.
        /// </summary>
        /// <param name="endpoint">Base address targeted by the HttpClient</param>
        /// <param name="applicationKey">Application key identifying the tenant</param>
        /// <param name="secretKey">Secret key for the tenant</param>
        /// <returns>Value representing a unique cache key</returns>
        private static string KeyFor(Uri endpoint, string applicationKey, string secretKey)
        {
            return $"{endpoint.AbsoluteUri.ToLowerInvariant()}:{applicationKey}:{secretKey}";
        }
    }
}
