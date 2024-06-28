namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceContentRepository : IGraphSourceRepository
    {
        Task SaveAsync<T>(Func<T, string> generateId, T data) where T : class, new();

        Task DeleteAsync(string id);
    }
}
