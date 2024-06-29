using Optimizely.Graph.Source.Sdk.ExpressionHelper;
using System.Linq.Expressions;

namespace Optimizely.Graph.Source.Sdk.Model
{
    public class SourceConfigurationModel
    {
        private static IDictionary<string, ContentTypeFieldConfiguration> _contentTypeFieldsConfigurations = new Dictionary<string, ContentTypeFieldConfiguration>();
        private static HashSet<string> _languages = new HashSet<string>();

        public static void AddLanguage(string language)
        {
            _languages.Add(language);
        }

        public static IEnumerable<string> GetLanguages() { return _languages; }

        public IEnumerable<ContentTypeFieldConfiguration> GetContentTypeFieldConfiguration()
        {
            return _contentTypeFieldsConfigurations.Values.ToArray();
        }

        public SourceConfigurationModel Field(Type type, Expression<Func<object, object>> fieldSelector, IndexingType indexingType)
        {
            AddField(type, fieldSelector, indexingType);
            return this;
        }

        protected SourceConfigurationModel AddField(Type type, Expression fieldSelector, IndexingType indexingType)
        {
            ContentTypeFieldConfiguration contentTypeFieldConfiguration;
            if (!_contentTypeFieldsConfigurations.TryGetValue(type.Name, out contentTypeFieldConfiguration))
            {
                contentTypeFieldConfiguration = new ContentTypeFieldConfiguration(type);
                _contentTypeFieldsConfigurations.Add(type.Name, contentTypeFieldConfiguration);
            }

            var fieldName = ExpressionExtensions.GetFieldPath(fieldSelector);
            var fieldType = ExpressionExtensions.GetReturnType(fieldSelector);
            var mappedTypeName = GetTypeName(fieldType);

            contentTypeFieldConfiguration.Fields.Add(new FieldInfo
            {
                Name = fieldName,
                IndexingType = indexingType,
                MappedType = mappedTypeName
            });

            return this;
        }

        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            if(!_contentTypeFieldsConfigurations.ContainsKey(type.Name))
            {
                throw new NotSupportedException($"The type {type.Name} has not been configured. Please configure it and try again.");
            }

            return _contentTypeFieldsConfigurations[type.Name].Fields;
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

    public class SourceConfigurationModel<T> : SourceConfigurationModel
        where T : class, new()
    {
        public SourceConfigurationModel<T> Field(Expression<Func<T, object>> fieldSelector, IndexingType indexingType)
        {
            AddField(typeof(T), fieldSelector, indexingType);
            return this;
        }

        public IEnumerable<FieldInfo> GetFields()
        {
            return GetFields(typeof(T));
        }
    }
}
