using System.Collections.Concurrent;

namespace Optimizely.Graph.Source.Sdk.HttpClientHelpers
{
    /// <summary>
    /// The CachingHttpClientFactory class constructs instances of
    /// HttpClient capable of communicating with an identified service.
    /// The factory delivers HttpClient instances from a cache if one
    /// has already been constructed for a particular service.
    /// </summary>
    /// <remarks>
    /// HttpClient instances are reentrant and thread safe. Pull
    /// clients from the cache to prevent socket consumption issues
    /// upon frequent access.
    /// </remarks>
    public class CachingHttpClientFactory : IHttpClientFactory
    {
        private static readonly ConcurrentDictionary<string, HttpClient> clients;
        private readonly IHttpClientFactory innerFactory;
        private readonly Func<Uri, HttpClientCacheKey> createCacheKey;

        /// <summary>
        /// Constructor
        /// </summary>
        static CachingHttpClientFactory()
        {
            clients = new ConcurrentDictionary<string, HttpClient>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="innerFactory">Inner HTTP client factory</param>
        /// <param name="createCacheKey">Creates a cache key, given an endpoint, for an HTTP client</param>
        public CachingHttpClientFactory(IHttpClientFactory innerFactory, Func<Uri, HttpClientCacheKey> createCacheKey)
        {
            this.innerFactory = innerFactory ?? throw new ArgumentNullException(nameof(innerFactory));
            this.createCacheKey = createCacheKey ?? throw new ArgumentNullException(nameof(createCacheKey));
        }

        /// <summary>
        /// Creates an instance of HttpClient targeting the service address
        /// identified for this factory. If a client for this service has
        /// previously been requested, a cached instance will be returned.
        /// </summary>
        /// <param name="endpoint">Uri endpoint.</param>
        /// <returns>HTTP client target the specified base address.</returns>
        public HttpClient Create(Uri endpoint)
        {
            var cacheKey = createCacheKey(endpoint);
            return clients.GetOrAdd(cacheKey.ToString(), key => innerFactory.Create(endpoint));
        }
    }
}