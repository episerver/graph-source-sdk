using Optimizely.Graph.Source.Sdk.JsonConverters;
using System.Text.Json;
using Optimizely.Graph.Source.Sdk.ContentGraph;
using Optimizely.Graph.Source.Sdk.Models;

namespace Optimizely.Graph.Source.Sdk.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphSourceRepository : IGraphSourceRepository
    {
        private readonly IContentGraphClient client;

        public GraphSourceRepository(IContentGraphClient client)
        {
            this.client = client;
        }

        public void AddLanguage(string language)
        {
            SourceConfigurationModel.AddLanguage(language);
        }

        public SourceConfigurationModel<T> ConfigureContentType<T>()
            where T : class, new()
        {
            return new SourceConfigurationModel<T>(ConfigurationType.ContentType);
        }

        public SourceConfigurationModel<T> ConfigurePropertyType<T>()
            where T : class, new()
        {
            return new SourceConfigurationModel<T>(ConfigurationType.PropertyType);
        }

        public async Task<string> SaveTypesAsync()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new SourceSdkContentTypeConverter()
                }
            };

            var jsonString = JsonSerializer.Serialize(SourceConfigurationModel.GetTypeFieldConfiguration(), serializeOptions);

            return await client.SendTypesAsync(jsonString);
        }

        public async Task<string> SaveContentAsync<T>(Func<T, string> generateId, params T[] data)
            where T : class, new()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new SourceSdkContentConverter()
                }
            };
            
            var itemJson = string.Empty;
            foreach (var item in data)
            {
                var id = generateId(item);
                var language = "en";

                itemJson += $"{{ \"index\": {{ \"_id\": \"{id}\", \"language_routing\": \"{language}\" }} }}";
                itemJson += Environment.NewLine;
                itemJson += JsonSerializer.Serialize(item, serializeOptions).Replace("\r\n", "");
                itemJson += Environment.NewLine;
            }

            var result = await client.SendContentBulkAsync(itemJson);
            return result;
        }

        public Task<string> DeleteContentAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
