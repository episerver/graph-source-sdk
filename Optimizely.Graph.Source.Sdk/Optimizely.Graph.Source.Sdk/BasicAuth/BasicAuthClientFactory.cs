using Optimizely.Graph.Source.Sdk.RestClientHelpers;

namespace Optimizely.Graph.Source.Sdk.BasicAuth
{
    /// <summary>
    /// Client factory to create a RestClient that performs basic authentication.
    /// </summary>
    public class BasicAuthClientFactory : IBasicAuthClientFactory
    {
        protected string appKey;
        protected string secret;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="secret">The application secret.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BasicAuthClientFactory(string appKey, string secret)
        {
            this.appKey = appKey ?? throw new ArgumentNullException(nameof(appKey));
            this.secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        /// <inheritdoc/>
        public IRestClient Create(IRestClient inner)
        {
            return new BasicAuthClient(inner, appKey, secret);
        }
    }
}
