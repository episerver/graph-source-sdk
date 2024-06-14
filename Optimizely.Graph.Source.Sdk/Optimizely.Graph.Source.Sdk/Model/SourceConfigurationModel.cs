using Optimizely.Graph.Source.Sdk.ExpressionHelper;
using System.Linq.Expressions;

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
            var fieldType = ExpressionExtensions.GetReturnType(fieldSelector);
            var mappedTypeName = GetTypeName(fieldType);

            contentTypeFieldConfiguration.Fields.Add(new FieldInfo {
                Name = fieldName,
                IndexingType = indexingType,
                MappedType = mappedTypeName 
            });

            return this;
        }

        public IEnumerable<FieldInfo> GetFields()
        {
            return _contentTypeFieldsConfigurations[typeof(T).Name].Fields;
        }

        private string GetTypeName(Type fieldType)
        {
            if (typeof(bool).IsAssignableFrom(fieldType))
            {
                return "Boolean";
            }
            else if (typeof(IEnumerable<bool>).IsAssignableFrom(fieldType))
            {
                return "[Boolean]";
            }
            else if (typeof(DateTime).IsAssignableFrom(fieldType))
            {
                return "DateTime";
            }
            else if (typeof(IEnumerable<DateTime>).IsAssignableFrom(fieldType))
            {
                return "[DateTime]";
            }
            else if (typeof(int).IsAssignableFrom(fieldType))
            {
                return "Int";
            }
            else if (typeof(IEnumerable<int>).IsAssignableFrom(fieldType))
            {
                return "[Int]";
            }
            else if (typeof(double).IsAssignableFrom(fieldType))
            {
                return "Float";
            }
            else if (typeof(IEnumerable<double>).IsAssignableFrom(fieldType))
            {
                return "[Float]";
            }
            else if (typeof(string).IsAssignableFrom(fieldType))
            {
                return "String";
            }
            else if (typeof(IEnumerable<string>).IsAssignableFrom(fieldType))
            {
                return "[String]";
            }

            throw new NotImplementedException();
        }
    }
}
