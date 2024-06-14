using Optimizely.Graph.Source.Sdk.Model;

namespace Optimizely.Graph.Source.Sdk.Repository
{
    public class DefaultGraphSourceRepository : IGraphSourceContentRepository, IGraphSourceTypeRepository
    {
        public string AppKey { get; private set; }

        public string Secret { get; private set; }

        public string BaseUrl { get; private set; }

        public DefaultGraphSourceRepository(string baseUrl, string appKey, string secret)
        {
            BaseUrl = baseUrl;
            AppKey = appKey;
            Secret = secret;
        }

        public SourceConfigurationModel<T> Confiture<T>()
        {
            return new SourceConfigurationModel<T>();
        }

        public async Task SaveAsync<T>(Func<T, string> generateId, T data)
        {
            var id = generateId(data);

            var typeConfiguration = new SourceConfigurationModel<T>();
            var fields = typeConfiguration.GetFields();

            // TODO: Generate content json
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveTypeAsync<T>()
        {
            // Check for configuration
            var typeConfiguration = new SourceConfigurationModel<T>();
            var fields = typeConfiguration.GetFields();
        
            // TODO: Generate types json
        }
    }
}
