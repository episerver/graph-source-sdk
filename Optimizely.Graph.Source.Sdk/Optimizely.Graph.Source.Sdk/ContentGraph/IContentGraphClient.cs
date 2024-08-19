
namespace Optimizely.Graph.Source.Sdk.ContentGraph
{
    internal interface IContentGraphClient
    {
        Task<string> SendTypesAsync(string typeJson);

        Task<string> SendContentBulkAsync(string json);
    }
}
