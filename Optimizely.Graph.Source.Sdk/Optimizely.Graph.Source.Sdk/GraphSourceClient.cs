using Optimizely.Graph.Source.Sdk.BasicAuth;
using Optimizely.Graph.Source.Sdk.HttpClientHelpers;
using Optimizely.Graph.Source.Sdk.RestClientHelpers;
using Optimizely.Graph.Source.Sdk.Repositories;
using Optimizely.Graph.Source.Sdk.SourceConfiguration;

namespace Optimizely.Graph.Source.Sdk
{
    public class GraphSourceClient
    {
        internal IGraphSourceRepository repository;

        private GraphSourceClient(IGraphSourceRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Creates an instance of GraphSourceClient.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="source"></param>
        /// <param name="appKey"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static GraphSourceClient Create(Uri baseUrl, string source, string appKey, string secret)
        {
            var httpClientFactory = CreateHttpClientFactory(appKey, secret);
            var contentGraphClientFactory = new BasicAuthClientFactory(appKey, secret);
            var restClient = new RestClient(httpClientFactory, baseUrl);
            var repository = new GraphSourceRepository(contentGraphClientFactory.Create(restClient), source);
            return new GraphSourceClient(repository);
        }

        public void AddLanguage(string language)
        {
            repository.AddLanguage(language);
        }

        public SourceConfigurationModel<T> ConfigureContentType<T>()
            where T : class, new()
        {
            return repository.ConfigureContentType<T>();
        }

        public SourceConfigurationModel<T> ConfigurePropertyType<T>()
            where T : class, new()
        {
            return repository.ConfigurePropertyType<T>();
        }

        public async Task<string> SaveTypesAsync()
        {
            return await repository.SaveTypesAsync();
        }

        public async Task<string> SaveContentAsync<T>(Func<T, string> generateId, params T[] data)
            where T : class, new()
        {
            return await repository.SaveContentAsync(generateId, data);
        }

        public async Task<string> DeleteContentAsync(string id)
        {
            return await repository.DeleteContentAsync(id);
        }

        /// <summary>
        /// Creates the HttpClientFactory for the REST services.
        /// </summary>
        /// <param name="appKey">Application key identifying the tenant</param>
        /// <param name="secretKey">Secret key for the tenant</param>
        /// <returns>CachingHttpClientFactory</returns>
        private static IHttpClientFactory CreateHttpClientFactory(string appKey, string secretKey)
        {
            Assert(appKey, secretKey);

            return new CachingHttpClientFactory(
                new HttpClientFactory(),
                uri => new AuthenticatingHttpClientCacheKey(uri, appKey, secretKey)
            );
        }

        /// <summary>
        /// Asserts the values provided.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="secretKey">The secret key.</param>
        private static void Assert(string appKey, string secretKey)
        {
            if (String.IsNullOrWhiteSpace(appKey))
                throw new ArgumentException("The value must contain at least one non-whitespace character.", nameof(appKey));

            if (String.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentException("The value must contain at least one non-whitespace character.", nameof(secretKey));
        }
    }
}
