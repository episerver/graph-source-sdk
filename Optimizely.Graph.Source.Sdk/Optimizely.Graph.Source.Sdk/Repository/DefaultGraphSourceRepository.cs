using Optimizely.Graph.Source.Sdk.Model;
using Optimizely.Graph.Source.Sdk.JsonConverter;
using System.Text.Json;
using System.Text;

namespace Optimizely.Graph.Source.Sdk.Repository
{
    public class DefaultGraphSourceRepository : IGraphSourceContentRepository, IGraphSourceTypeRepository
    {
        private const string TypeUrl = "/api/content/v3/types";
        private const string DataUrl = "/api/content/v2/data";

        public string AppKey { get; private set; }

        public string Secret { get; private set; }

        public string BaseUrl { get; private set; }

        public string Source { get; private set; }

        public DefaultGraphSourceRepository(string baseUrl, string source, string appKey, string secret)
        {
            BaseUrl = baseUrl;
            AppKey = appKey;
            Secret = secret;
            Source = source;
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

        public async Task<string> SaveAsync<T>(Func<T, string> generateId, params T[] data)
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

            var result = await SendContentBulk(itemJson);
            return result;
        }

        public Task<string> DeleteAsync(string id)
        {
            throw new NotImplementedException();
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

            return await SendTypes(jsonString);
        }

        private async Task<string> SendTypes(string typeJson)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GetBasicAuthString());

            var content = new StringContent(typeJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{BaseUrl}/{TypeUrl}?id={Source}", content);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private async Task<string> SendContentBulk(string json)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GetBasicAuthString());

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{BaseUrl}/{DataUrl}?id={Source}", content);
            return await response.Content.ReadAsStringAsync();
        }

        private string GetBasicAuthString()
        {
            return $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppKey}:{Secret}"))}";
        }
    }
}
