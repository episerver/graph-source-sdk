using Optimizely.Graph.Source.Sdk.RestClientHelpers;

namespace Optimizely.Graph.Source.Sdk.BasicAuth
{
    /// <summary>
    /// Interface that describes a factory that creates a rest client with Basic Authorization.
    /// </summary>
    public interface IBasicAuthClientFactory
    {
        /// <summary>
        /// Create a rest client with basic authorization.
        /// </summary>
        /// <param name="inner">Inner Rest client.</param>
        /// <returns></returns>
        IRestClient Create(IRestClient inner);
    }
}
