using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Optimizely.Graph.Source.Sdk.Expression;

namespace Optimizely.Graph.Source.Sdk.Model
{
    public class SourceConfigurationModel<T>
    {
        private static IDictionary<string, ContentTypeFieldConfiguration> _contentTypeFieldsConfigurations = new Dictionary<string, ContentTypeFieldConfiguration>();

        public SourceConfigurationModel<T> Field(Expression<Func<T, object>> fieldSelector, IndexingType indexingType)
        {
            ContentTypeFieldConfiguration contentTypeFieldConfiguration;
            if(!_contentTypeFieldsConfigurations.TryGetValue(typeof(T).Name, out contentTypeFieldConfiguration))
            {
                contentTypeFieldConfiguration = new ContentTypeFieldConfiguration(typeof(T));
                _contentTypeFieldsConfigurations.Add(typeof(T).Name, contentTypeFieldConfiguration);
            }

            var fieldName = ExpressionExtensions.GetFieldPath(fieldSelector);
            contentTypeFieldConfiguration.Fields.Add(fieldName, indexingType);

            return this;
        }

        public IDictionary<string, IndexingType> GetFields()
        {
            return _contentTypeFieldsConfigurations[typeof(T).Name].Fields;
        }
    }
}
