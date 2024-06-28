namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceContentRepository : IGraphSourceRepository
    {
        Task<string> SaveAsync<T>(Func<T, string> generateId, T data) where T : class, new();

        Task<string> DeleteAsync(string id);
    }
}
