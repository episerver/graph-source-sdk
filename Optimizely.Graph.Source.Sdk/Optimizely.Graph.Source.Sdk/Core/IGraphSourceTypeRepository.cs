using Optimizely.Graph.Source.Sdk.Core.Models;

namespace Optimizely.Graph.Source.Sdk.Core
{
    public interface IGraphSourceTypeRepository : IGraphSourceRepository
    {
        SourceConfigurationModel<T> ConfigureContentType<T>() where T : class, new();

        Task<string> SaveTypesAsync();

        void AddLanguage(string language);
    }
}
