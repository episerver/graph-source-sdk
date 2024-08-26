using Optimizely.Graph.Source.Sdk.RestClientHelpers;

namespace Optimizely.Graph.Source.Sdk.BasicAuth
{
    /// <summary>
    /// An interface that describes a factory that creates a http client for Content Graph.
    /// </summary>
    public interface IBasicAuthClientFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inner"></param>
        /// <returns></returns>
        IRestClient Create(IRestClient inner);
    }
}
