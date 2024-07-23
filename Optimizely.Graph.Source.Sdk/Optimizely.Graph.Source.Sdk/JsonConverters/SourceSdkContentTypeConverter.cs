using System.Text.Json.Serialization;
using System.Text.Json;
using Optimizely.Graph.Source.Sdk.Core.Models;

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

            foreach(var contentTypeFieldConfiguration in value.Where(x => x.ConfigurationType == ConfigurationType.ContentType))
            {
                writer.WriteBoolean("useTypedFieldNames", true);
                writer.WriteString("label", contentTypeFieldConfiguration.TypeName);

                writer.WriteStartArray("languages");
                foreach (var language in SourceConfigurationModel.GetLanguages())
                {
                    writer.WriteStringValue(language);
                }
                writer.WriteEndArray();

                writer.WriteStartObject("contentTypes");
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
                writer.WriteEndObject();
            }

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
    }
}
