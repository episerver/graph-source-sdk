using Optimizely.Graph.Source.Sdk.SourceConfiguration;

namespace Optimizely.Graph.Source.Sdk.Repositories
{
    public interface IGraphSourceRepository
    {
        void AddLanguage(string language);

        SourceConfigurationModel<T> ConfigureContentType<T>() where T : class, new();

        SourceConfigurationModel<T> ConfigurePropertyType<T>() where T : class, new();

        Task<string> SaveTypesAsync();

        Task<string> SaveContentAsync<T>(Func<T, string> generateId, params T[] data) where T : class, new();

        Task<string> DeleteContentAsync(string id);
    }
}
