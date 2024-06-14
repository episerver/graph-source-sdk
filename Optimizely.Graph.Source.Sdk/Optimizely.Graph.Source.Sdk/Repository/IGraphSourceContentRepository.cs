namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceContentRepository 
    {
        Task SaveAsync<T>(Func<T, string> generateId, T data);

        Task DeleteAsync(string id);
    }
}
