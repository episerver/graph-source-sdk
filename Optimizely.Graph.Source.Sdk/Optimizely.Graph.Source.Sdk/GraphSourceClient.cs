using Optimizely.Graph.Source.Sdk.ContentGraph;
using Optimizely.Graph.Source.Sdk.Models;
using Optimizely.Graph.Source.Sdk.Repositories;

namespace Optimizely.Graph.Source.Sdk
{
    public class GraphSourceClient
    {
        internal IGraphSourceRepository _repository;

        private GraphSourceClient(IGraphSourceRepository repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Creates an instance of GraphSourceClient.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="source"></param>
        /// <param name="appKey"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static GraphSourceClient Create(string baseUrl, string source, string appKey, string secret)
        {
            IContentGraphClientFactory contentGraphClientFactory = new ContentGraphClientFactory(baseUrl, source, appKey, secret);
            var repository = new GraphSourceRepository(contentGraphClientFactory.Create());
            return new GraphSourceClient(repository);
        }

        public void AddLanguage(string language)
        {
            _repository.AddLanguage(language);
        }

        public SourceConfigurationModel<T> ConfigureContentType<T>()
            where T : class, new()
        {
            return _repository.ConfigureContentType<T>();
        }

        public SourceConfigurationModel<T> ConfigurePropertyType<T>()
            where T : class, new()
        {
            return _repository.ConfigurePropertyType<T>();
        }

        public async Task<string> SaveTypesAsync()
        {
            return await _repository.SaveTypesAsync();
        }

        public async Task<string> SaveContentAsync<T>(Func<T, string> generateId, params T[] data)
            where T : class, new()
        {
            return await _repository.SaveContentAsync(generateId, data);
        }

        public async Task<string> DeleteContentAsync(string id)
        {
            return await _repository.DeleteContentAsync(id);
        }
    }
}
