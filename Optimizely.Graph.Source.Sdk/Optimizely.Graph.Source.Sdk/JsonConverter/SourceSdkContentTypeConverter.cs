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
    public class SourceSdkContentTypeConverter : JsonConverter<IEnumerable<ContentTypeFieldConfiguration>>
    {
        public override IEnumerable<ContentTypeFieldConfiguration>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<ContentTypeFieldConfiguration> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach(var contentTypeFieldConfiguration in value)
            {
                writer.WriteBoolean("useTypedFieldNames", true);
                writer.WriteString("label", contentTypeFieldConfiguration.ContentTypeName);

                writer.WriteStartArray("languages");
                foreach (var language in SourceConfigurationModel.GetLanguages())
                {
                    writer.WriteStringValue(language);
                }
                writer.WriteEndArray();

                writer.WriteStartObject("contentTypes");
                writer.WriteStartObject(contentTypeFieldConfiguration.ContentTypeName);

                writer.WriteStartArray("contentType");
                writer.WriteEndArray();

                writer.WriteStartObject("properties");

                foreach (var field in contentTypeFieldConfiguration.Fields)
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
            }
            

            writer.WriteEndObject();
        }
    }
}
