namespace Optimizely.Graph.Source.Sdk.Core
{
    public interface IGraphSourceContentRepository : IGraphSourceRepository
    {
        Task<string> SaveAsync<T>(Func<T, string> generateId, params T[] data) where T : class, new();

        Task<string> DeleteAsync(string id);
    }
}
