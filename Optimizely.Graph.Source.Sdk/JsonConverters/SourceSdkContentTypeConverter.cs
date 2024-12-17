using System.Text.Json.Serialization;
using System.Text.Json;
using Optimizely.Graph.Source.Sdk.SourceConfiguration;

namespace Optimizely.Graph.Source.Sdk.JsonConverters
{
    public class SourceSdkContentTypeConverter : JsonConverter<IEnumerable<TypeFieldConfiguration>>
    {
        public override IEnumerable<TypeFieldConfiguration>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<TypeFieldConfiguration> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteBoolean("useTypedFieldNames", true);

            writer.WriteStartArray("languages");
            foreach (var language in SourceConfigurationModel.GetLanguages())
            {
                writer.WriteStringValue(language);
            }
            writer.WriteEndArray();

            // Links
            writer.WriteStartObject("links");
            foreach (var link in value.SelectMany(x => x.GraphLinks))
            {
                writer.WriteStartObject(link.Name);

                writer.WriteString("from", GetFieldName(link.From));
                writer.WriteString("to", GetFieldName(link.To));

                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            writer.WriteStartObject("contentTypes");
            foreach (var contentTypeFieldConfiguration in value.Where(x => x.ConfigurationType == ConfigurationType.ContentType))
            {
                writer.WriteStartObject(contentTypeFieldConfiguration.TypeName);

                writer.WriteStartArray("contentType");
                writer.WriteEndArray();

                writer.WriteStartObject("properties");
                foreach (var field in contentTypeFieldConfiguration.Fields)
                {
                    writer.WriteStartObject(field.Name);

                    writer.WriteString("type", field.MappedTypeName);
                    if(field.IndexingType != IndexingType.PropertyType)
                    {
                        writer.WriteBoolean("searchable", field.IndexingType == IndexingType.Searchable);
                        writer.WriteBoolean("skip", field.IndexingType == IndexingType.OnlyStored);
                    }

                    writer.WriteEndObject();
                }
                writer.WriteEndObject();

                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            writer.WriteStartObject("propertyTypes");
            foreach (var contentTypeFieldConfiguration in value.Where(x => x.ConfigurationType == ConfigurationType.PropertyType))
            {
                writer.WriteStartObject(contentTypeFieldConfiguration.TypeName);

                writer.WriteStartObject("properties");

                foreach (var field in contentTypeFieldConfiguration.Fields)
                {
                    writer.WriteStartObject(field.Name);

                    writer.WriteString("type", field.MappedTypeName);
                    if (field.IndexingType != IndexingType.PropertyType)
                    {
                        writer.WriteBoolean("searchable", field.IndexingType == IndexingType.Searchable);
                        writer.WriteBoolean("skip", field.IndexingType == IndexingType.OnlyStored);
                    }

                    writer.WriteEndObject();
                }

                writer.WriteEndObject();

                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        private string GetFieldName(FieldInfo fieldInfoItem)
        {
            var fieldName = fieldInfoItem.Name;
            switch(fieldInfoItem.MappedTypeName)
            {
                case "[Boolean]":
                case "Boolean":
                    {
                        fieldName += "$$Boolean";
                        break;
                    }
                case "[DateTime]":
                case "DateTime":
                    {
                        fieldName += "$$DateTime";
                        break;
                    }
                case "[Int]":
                case "Int":
                    {
                        fieldName += "$$Int";
                        break;
                    }
                case "[Float]":
                case "Float":
                    {
                        fieldName += "$$Float";
                        break;
                    }
                case "[String]":
                case "String":
                    {
                        fieldName += "$$String";
                        break;
                    }
            }

            switch(fieldInfoItem.IndexingType)
            {
                case IndexingType.OnlyStored:
                    {
                        fieldName += "___skip";
                        break;
                    }
                case IndexingType.Searchable:
                    {
                        fieldName += "___searchable";
                        break;
                    }
            }

            return fieldName;
        }
    }
}
