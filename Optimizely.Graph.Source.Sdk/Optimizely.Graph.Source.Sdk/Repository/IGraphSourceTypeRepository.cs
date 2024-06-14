using Optimizely.Graph.Source.Sdk.Model;

namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceTypeRepository : IGraphSourceRepository
    {
        SourceConfigurationModel<T> Configure<T>();

        Task SaveTypeAsync<T>();
    }
}
