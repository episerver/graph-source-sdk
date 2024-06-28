using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Optimizely.Graph.Source.Sdk.Model;

namespace Optimizely.Graph.Source.Sdk.JsonConverter
{
    public class ContentTypeModelConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var contentType = value.GetType();
            var contentTypeName = contentType.Name;

            writer.WriteBoolean("useTypedFieldNames", true);
            writer.WriteString("label", contentTypeName);

            writer.WriteStartArray("languages");
            writer.WriteStringValue("en");
            writer.WriteEndArray();

            writer.WriteStartObject("contentTypes");
            writer.WriteStartObject(contentTypeName);

            writer.WriteStartArray("contentType");
            writer.WriteEndArray();

            writer.WriteStartObject("properties");

            var typeConfiguration = new SourceConfigurationModel();
            var fields = typeConfiguration.GetFields(contentType);
            foreach (var field in fields)
            {
                writer.WriteStartObject(field.Name);

                writer.WriteString("type", field.MappedType);
                writer.WriteBoolean("searchable", field.IndexingType == IndexingType.Searchable);
                writer.WriteBoolean("skip", field.IndexingType == IndexingType.OnlyStored);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
