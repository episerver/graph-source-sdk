using System.Text;

namespace Optimizely.Graph.Source.Sdk.ContentGraph
{
    internal class ContentGraphClient : IContentGraphClient
    {
        private const string TypeUrl = "/api/content/v3/types";
        private const string DataUrl = "/api/content/v2/data";

        private readonly HttpClient client;
        private readonly string baseUrl;
        private readonly string source;

        public ContentGraphClient(HttpClient client, string baseUrl, string source)
        {
            this.client = client;
            this.baseUrl = baseUrl;
            this.source = source;
        }

        public async Task<string> SendTypesAsync(string typeJson)
        {
            var content = new StringContent(typeJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/{TypeUrl}?id={source}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendContentBulkAsync(string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/{DataUrl}?id={source}", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
