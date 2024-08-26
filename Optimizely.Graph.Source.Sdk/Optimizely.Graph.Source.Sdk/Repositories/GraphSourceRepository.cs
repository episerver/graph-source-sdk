using Optimizely.Graph.Source.Sdk.JsonConverters;
using System.Text.Json;
using System.Text;
using Optimizely.Graph.Source.Sdk.RestClientHelpers;
using Optimizely.Graph.Source.Sdk.SourceConfiguration;

namespace Optimizely.Graph.Source.Sdk.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphSourceRepository : IGraphSourceRepository
    {
        private readonly IRestClient client;
        private readonly string source;

        private const string TypeUrl = "/api/content/v3/types";
        private const string DataUrl = "/api/content/v2/data";

        public GraphSourceRepository(IRestClient client, string source)
        {
            this.client = client;
            this.source = source;
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

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{TypeUrl}?id={source}"))
            {
                requestMessage.Content = content;
                using (var responseMessage = await client.SendAsync(requestMessage))
                {
                    await client.HandleResponse(responseMessage);
                }
            }

            return string.Empty;
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

            var content = new StringContent(itemJson, Encoding.UTF8, "application/json");

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{DataUrl}?id={source}"))
            {
                requestMessage.Content = content;
                using (var responseMessage = await client.SendAsync(requestMessage))
                {
                    await client.HandleResponse(responseMessage);
                }
            }
            return string.Empty;
        }

        public Task<string> DeleteContentAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
