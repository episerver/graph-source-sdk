using Microsoft.VisualBasic.FileIO;
using Optimizely.Graph.Source.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.JsonConverter
{
    public class SourceSdkContentConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.GetType() != typeof(IEnumerable<TypeFieldConfiguration>);
        }

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var contentType = value.GetType();

            var fields = SourceConfigurationModel.GetContentFields(contentType);

            //WriteMetaData(writer, value, options, contentType, fields);
            WriteContent(writer, value, options, contentType, fields);
        }

        void WriteMetaData(Utf8JsonWriter writer, object value, JsonSerializerOptions options, Type contentType, IEnumerable<FieldInfo> fieldInfoItems)
        {
            writer.WriteStartObject();
            writer.WriteStartObject("index");

            writer.WriteString("_id", "testar"); // TODO: Set Id
            writer.WriteString("language_routing", "en"); // TODO: Set Language

            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        void WriteContent(Utf8JsonWriter writer, object value, JsonSerializerOptions options, Type contentType, IEnumerable<FieldInfo> fieldInfoItems)
        {
            writer.WriteStartObject();

            writer.WriteString("Status$$String", "Published");
            writer.WriteString("__typename", contentType.Name);
            writer.WriteString("_rbac", "r:Everyone:Read");
            writer.WriteString("ContentType$$String", contentType.Name);

            writer.WriteStartObject("Language");
            writer.WriteString("Name$$String", "en"); //TODO: Set Language
            writer.WriteEndObject();

            foreach(var fieldInfoItem in fieldInfoItems)
            {
                var fieldValue = contentType.GetProperty(fieldInfoItem.Name).GetValue(value);
                WriteField(writer, fieldValue, fieldInfoItem);
            }

            writer.WriteEndObject();
        }

        private static void WriteField(Utf8JsonWriter writer, object fieldValue, FieldInfo fieldInfoItem)
        {
            if(fieldInfoItem.IndexingType == IndexingType.PropertyType)
            {
                writer.WriteStartObject(fieldInfoItem.Name);

                var type = fieldInfoItem.MappedType.GetProperty(fieldInfoItem.Name).PropertyType;
                var fields = SourceConfigurationModel.GetPropertyFields(type);
                foreach( var field in fields)
                {
                    var propertyFieldValue = type.GetProperty(field.Name).GetValue(fieldValue);
                    WriteField(writer, propertyFieldValue, field);
                }

                writer.WriteEndObject();
            }

            if (fieldInfoItem.MappedTypeName == "Boolean")
            {
                writer.WriteBoolean(fieldInfoItem.ToString(), (bool)fieldValue);
            }
            else if (fieldInfoItem.MappedTypeName == "[Boolean]")
            {
                writer.WriteStartArray(fieldInfoItem.ToString());
                foreach (var item in (IEnumerable<bool>)fieldValue)
                {
                    writer.WriteBooleanValue(item);
                }
                writer.WriteEndArray();
            }
            else if (fieldInfoItem.MappedTypeName == "DateTime")
            {
                writer.WriteString(fieldInfoItem.ToString(), ((DateTime)fieldValue).ToUniversalTime().ToString());
            }
            else if (fieldInfoItem.MappedTypeName == "[DateTime]")
            {
                writer.WriteStartArray(fieldInfoItem.ToString());
                foreach (var item in (IEnumerable<DateTime>)fieldValue)
                {
                    writer.WriteStringValue(((DateTime)fieldValue).ToUniversalTime().ToString());
                }
                writer.WriteEndArray();
            }
            else if (fieldInfoItem.MappedTypeName == "Int")
            {
                writer.WriteNumber(fieldInfoItem.ToString(), (int)fieldValue);
            }
            else if (fieldInfoItem.MappedTypeName == "[Int]")
            {
                writer.WriteStartArray(fieldInfoItem.ToString());
                foreach (var item in (IEnumerable<float>)fieldValue)
                {
                    writer.WriteNumberValue(item);
                }
                writer.WriteEndArray();
            }
            else if (fieldInfoItem.MappedTypeName == "Float")
            {
                writer.WriteNumber(fieldInfoItem.ToString(), (float)fieldValue);
            }
            else if (fieldInfoItem.MappedTypeName == "[Float]")
            {
                writer.WriteStartArray(fieldInfoItem.ToString());
                foreach (var item in (IEnumerable<float>)fieldValue)
                {
                    writer.WriteNumberValue(item);
                }
                writer.WriteEndArray();
            }
            else if (fieldInfoItem.MappedTypeName == "String")
            {
                writer.WriteString(fieldInfoItem.ToString(), (string)fieldValue);
            }
            else if (fieldInfoItem.MappedTypeName == "[String]")
            {
                writer.WriteStartArray(fieldInfoItem.ToString());
                foreach (var item in (IEnumerable<string>)fieldValue)
                {
                    writer.WriteStringValue(item);
                }
                writer.WriteEndArray();
            }
        }
    }
}
