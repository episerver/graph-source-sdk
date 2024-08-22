
namespace Optimizely.Graph.Source.Sdk.ContentGraph
{
    /// <summary>
    /// An interface that describes a factory that creates a http client for Content Graph.
    /// </summary>
    public interface IContentGraphClientFactory
    {
        /// <summary>
        /// Creates a http client for Content Graph.
        /// </summary>
        /// <returns></returns>
        IContentGraphClient Create();
    }
}
