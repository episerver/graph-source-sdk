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
        /// Creates an instance of GraphSourceClient targeting Content Graph REST services.
        /// </summary>
        /// <param name="baseUrl">Base url for Content Graph REST services.</param>
        /// <param name="source">Source for Content Graph REST services.</param>
        /// <param name="appKey">Application key for Content Graph REST services.</param>
        /// <param name="secret">Secret key for Content Graph REST services.</param>
        /// <returns>An instance of the Graph Source client.</returns>
        public static GraphSourceClient Create(Uri baseUrl, string source, string appKey, string secret)
        {
            var httpClientFactory = CreateHttpClientFactory(appKey, secret);
            var basicAuthClientFactory = CreateBasicAuthClientFactory(appKey, secret);

            var restClient = new RestClient(httpClientFactory, baseUrl);
            var repository = new GraphSourceRepository(basicAuthClientFactory.Create(restClient), source);

            return new GraphSourceClient(repository);
        }

        /// <summary>
        /// Adds language preference to SourceConfigurationModel.
        /// </summary>
        /// <param name="language"></param>
        public void AddLanguage(string language)
        {
            repository.AddLanguage(language);
        }

        /// <summary>
        /// Configures Content Types within the SourceConfigurationModel.
        /// </summary>
        /// <typeparam name="T">Generic content type.</typeparam>
        /// <returns></returns>
        public SourceConfigurationModel<T> ConfigureContentType<T>()
            where T : class, new()
        {
            return repository.ConfigureContentType<T>();
        }

        /// <summary>
        /// Configures Content Property Types within the SourceConfigurationModel.
        /// </summary>
        /// <typeparam name="T">Generic property type.</typeparam>
        /// <returns></returns>
        public SourceConfigurationModel<T> ConfigurePropertyType<T>()
            where T : class, new()
        {
            return repository.ConfigurePropertyType<T>();
        }

        /// <summary>
        /// Saves Content Types set in the SourceConfigurationModel to the Content Graph api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SaveTypesAsync()
        {
            return await repository.SaveTypesAsync();
        }

        /// <summary>
        /// Saves dynamic content sent in data array to the Content Graph api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="generateId">Id associated with content.</param>
        /// <param name="data">Dynamic data being saved to Content Graph.</param>
        /// <returns></returns>
        public async Task<string> SaveContentAsync<T>(Func<T, string> generateId, params T[] data)
            where T : class, new()
        {
            return await repository.SaveContentAsync(generateId, data);
        }

        /// <summary>
        /// Removes content previously saved from the Content Graph api.
        /// </summary>
        /// <param name="id">Id of the content being removed.</param>
        /// <returns></returns>
        public async Task<string> DeleteContentAsync(string id)
        {
            return await repository.DeleteContentAsync(id);
        }

        #region Private
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
        /// Creates the BasicAuthClientFactory for the REST services.
        /// </summary>
        /// <param name="appKey">Application key identifying the tenant</param>
        /// <param name="secretKey">Secret key for the tenant</param>
        /// <returns>BasicAuthClientFactory</returns>
        private static IBasicAuthClientFactory CreateBasicAuthClientFactory(string appKey, string secretKey)
        {
            return new BasicAuthClientFactory(appKey, secretKey);
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
        #endregion
    }
}
