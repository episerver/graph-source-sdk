using Optimizely.Graph.Source.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Repository
{
    public class DefaultGraphSourceRepository : IGraphSourceContentRepository, IGraphSourceTypeRepository
    {
        public SourceConfigurationModel<T> Confiture<T>()
        {
            return new SourceConfigurationModel<T>();
        }

        public async Task SaveAsync<T>(Func<T, string> generateId, T data)
        {
            var id = generateId(data);

            var typeConfiguration = new SourceConfigurationModel<T>();
            var fields = typeConfiguration.GetFields();
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
        }
    }
}
