using Optimizely.Graph.Source.Sdk.ExpressionHelpers;
using System.Linq.Expressions;

namespace Optimizely.Graph.Source.Sdk.SourceConfiguration
{
    public class ConfiguredGraphLink
    { 
        public string Name { get; set; }

        public FieldInfo From { get; set; }

        public FieldInfo To { get; set; }
    }

    public class SourceConfigurationModel
    {
        private static IDictionary<string, TypeFieldConfiguration> _contentTypeFieldsConfigurations = new Dictionary<string, TypeFieldConfiguration>();
        private static IDictionary<string, TypeFieldConfiguration> _propertyTypeFieldsConfigurations = new Dictionary<string, TypeFieldConfiguration>();
        //private static IDictionary<string, ConfiguredGraphLink> _configuredGraphLinks = new Dictionary<string, ConfiguredGraphLink>();
        private static HashSet<string> _languages = new HashSet<string>();

        protected ConfigurationType ConfigurationType { get; private set; }

        public SourceConfigurationModel(ConfigurationType configurationType)
        {
            ConfigurationType = configurationType;
        }

        public static void ConfigureLink<T, U>(string name, Expression<Func<T, object>> fromExpression, Expression<Func<U, object>> toExpression)
        {
            var fromType = typeof(T);
            var fromFieldName = fromExpression.GetFieldPath();
            var fromFieldType = fromExpression.GetReturnType();
            var fromField = _contentTypeFieldsConfigurations[fromType.Name].Fields.Single(x => x.Name == fromFieldName);

            var toType = typeof(U);
            var toFieldName = toExpression.GetFieldPath();
            var toFieldType = toExpression.GetReturnType();
            var toField = _contentTypeFieldsConfigurations[toType.Name].Fields.Single(x => x.Name == toFieldName);

            if (_contentTypeFieldsConfigurations.TryGetValue(fromType.Name, out TypeFieldConfiguration? contentTypeFieldConfiguration))
            {
                var graphLink = new ConfiguredGraphLink
                {
                    Name = name,
                    From = new FieldInfo
                    {
                        Name = fromFieldName,
                        IndexingType = fromField.IndexingType,
                        MappedType = fromType,
                        MappedTypeName = fromField.MappedTypeName
                    },
                    To = new FieldInfo
                    {
                        Name = toFieldName,
                        IndexingType = toField.IndexingType,
                        MappedType = toType,
                        MappedTypeName = toField.MappedTypeName
                    }
                };

                contentTypeFieldConfiguration.GraphLinks.Add(graphLink);
            }
        }

        public static void AddLanguage(string language)
        {
            _languages.Add(language);
        }

        public static IEnumerable<string> GetLanguages() { return _languages; }

        public static IEnumerable<TypeFieldConfiguration> GetTypeFieldConfiguration()
        {
            var typeFieldConfiguration = new List<TypeFieldConfiguration>();
            typeFieldConfiguration.AddRange(_contentTypeFieldsConfigurations.Values);
            typeFieldConfiguration.AddRange(_propertyTypeFieldsConfigurations.Values);

            return typeFieldConfiguration.ToArray();
        }

        public static IEnumerable<TypeFieldConfiguration> GetContentTypeFieldConfiguration()
        {
            return _contentTypeFieldsConfigurations.Values.ToArray();
        }

        public static IEnumerable<TypeFieldConfiguration> GetPropertyTypeFieldConfiguration()
        {
            return _propertyTypeFieldsConfigurations.Values.ToArray();
        }

        public SourceConfigurationModel Field(Type type, Expression<Func<object, object>> fieldSelector, IndexingType indexingType)
        {
            if (ConfigurationType == ConfigurationType.ContentType)
            {
                AddContentTypeField(type, fieldSelector, indexingType);
            }
            else
            {
                AddPropertyTypeField(type, fieldSelector, indexingType);
            }

            return this;
        }

        protected SourceConfigurationModel AddContentTypeField(Type type, Expression fieldSelector, IndexingType indexingType)
        {
            TypeFieldConfiguration contentTypeFieldConfiguration;
            if (!_contentTypeFieldsConfigurations.TryGetValue(type.Name, out contentTypeFieldConfiguration))
            {
                contentTypeFieldConfiguration = new TypeFieldConfiguration(type, ConfigurationType);
                _contentTypeFieldsConfigurations.Add(type.Name, contentTypeFieldConfiguration);
            }

            var fieldName = fieldSelector.GetFieldPath();
            var fieldType = fieldSelector.GetReturnType();
            var mappedTypeName = indexingType == IndexingType.PropertyType ? fieldType.Name : GetTypeName(fieldType);

            contentTypeFieldConfiguration.Fields.Add(new FieldInfo
            {
                Name = fieldName,
                IndexingType = indexingType,
                MappedType = type,
                MappedTypeName = mappedTypeName
            });

            return this;
        }

        protected SourceConfigurationModel AddPropertyTypeField(Type type, Expression fieldSelector, IndexingType indexingType)
        {
            TypeFieldConfiguration propertyTypeFieldConfiguration;
            if (!_propertyTypeFieldsConfigurations.TryGetValue(type.Name, out propertyTypeFieldConfiguration))
            {
                propertyTypeFieldConfiguration = new TypeFieldConfiguration(type, ConfigurationType);
                _propertyTypeFieldsConfigurations.Add(type.Name, propertyTypeFieldConfiguration);
            }

            var fieldName = fieldSelector.GetFieldPath();
            var fieldType = fieldSelector.GetReturnType();

            var mappedTypeName = string.Empty;
            if (indexingType == IndexingType.PropertyType && typeof(IEnumerable<object>).IsAssignableFrom(fieldType))
            {
                mappedTypeName = $"[{fieldType.GetGenericArguments()[0].Name}]";
            }
            else
            {
                mappedTypeName = indexingType == IndexingType.PropertyType ? fieldType.Name : GetTypeName(fieldType);
            }

            propertyTypeFieldConfiguration.Fields.Add(new FieldInfo
            {
                Name = fieldName,
                IndexingType = indexingType,
                MappedType = type,
                MappedTypeName = mappedTypeName
            });

            return this;
        }

        public static IEnumerable<FieldInfo> GetContentFields(Type type)
        {
            if (!_contentTypeFieldsConfigurations.ContainsKey(type.Name))
            {
                throw new NotSupportedException($"The type {type.Name} has not been configured. Please configure it and try again.");
            }

            return _contentTypeFieldsConfigurations[type.Name].Fields;
        }

        public static IEnumerable<FieldInfo> GetPropertyFields(Type type)
        {
            if (!_propertyTypeFieldsConfigurations.ContainsKey(type.Name))
            {
                throw new NotSupportedException($"The type {type.Name} has not been configured. Please configure it and try again.");
            }

            
            return _propertyTypeFieldsConfigurations[type.Name].Fields;
        }

        public static IEnumerable<FieldInfo> GetPropertyFieldsByName(string name)
        {
            if (!_propertyTypeFieldsConfigurations.ContainsKey(name))
            {
                throw new NotSupportedException($"The type {name} has not been configured. Please configure it and try again.");
            }


            return _propertyTypeFieldsConfigurations[name].Fields;
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
        public SourceConfigurationModel(ConfigurationType configurationType)
            : base(configurationType)
        {
        }

        public SourceConfigurationModel<T> Field(Expression<Func<T, object>> fieldSelector, IndexingType indexingType)
        {
            if (ConfigurationType == ConfigurationType.ContentType)
            {
                AddContentTypeField(typeof(T), fieldSelector, indexingType);
            }
            else
            {
                AddPropertyTypeField(typeof(T), fieldSelector, indexingType);
            }

            return this;
        }

        public IEnumerable<FieldInfo> GetFields()
        {
            return GetContentFields(typeof(T));
        }
    }
}
