using Optimizely.Graph.Source.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceTypeRepository
    {
        void Confiture<T>(Action<IDictionary<Expression<Func<T, object>>, IndexingType>> mappings);

        Task SaveTypeAsync<T>();
    }
}
