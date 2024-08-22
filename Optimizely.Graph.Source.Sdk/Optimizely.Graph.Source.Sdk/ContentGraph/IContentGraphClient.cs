
namespace Optimizely.Graph.Source.Sdk.ContentGraph
{
    public interface IContentGraphClient
    {
        Task<string> SendTypesAsync(string typeJson);

        Task<string> SendContentBulkAsync(string json);
    }
}
