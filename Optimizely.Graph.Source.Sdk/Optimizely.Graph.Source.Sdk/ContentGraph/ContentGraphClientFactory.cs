using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.ContentGraph
{
    public class ContentGraphClientFactory : IContentGraphClientFactory
    {
        private readonly string baseUrl;
        private readonly string source;
        private readonly string appKey;
        private readonly string secret;
        

        public ContentGraphClientFactory(string baseUrl, string source, string appKey, string secret)
        {
            this.baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.appKey = appKey ?? throw new ArgumentNullException(nameof(appKey));
            this.secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        public IContentGraphClient Create()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GetBasicAuthString());

            return new ContentGraphClient(client, baseUrl, source);
        }

        private string GetBasicAuthString()
        {
            return $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{appKey}:{secret}"))}";
        }
    }
}
