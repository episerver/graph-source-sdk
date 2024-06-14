namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceContentRepository : IGraphSourceRepository
    {
        Task SaveAsync<T>(Func<T, string> generateId, T data);

        Task DeleteAsync(string id);
    }
}
