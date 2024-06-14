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
        public void Confiture<T>(Action<IDictionary<Expression<Func<T, object>>, IndexingType>> mappings)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync<T>(Func<T, string> generateId, T data)
        {
            var id = generateId(data);

        }

        public async Task SaveTypeAsync<T>()
        {
            throw new NotImplementedException();
        }
    }
}
