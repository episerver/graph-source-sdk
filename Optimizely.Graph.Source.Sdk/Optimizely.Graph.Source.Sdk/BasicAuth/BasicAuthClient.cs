using System.Text;

namespace Optimizely.Graph.Source.Sdk.BasicAuth
{
    public class BasicAuthClient : IRestClient
    {
        private readonly IRestClient inner;
        public readonly string appKey;
        public readonly string secret;

        public BasicAuthClient(IRestClient inner, string appKey, string secret)
        {
            this.inner = inner;
            this.appKey = appKey;
            this.secret = secret;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", GetBasicAuthString());
            return await inner.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", GetBasicAuthString());
            return await inner.SendAsync(request, cancellationToken);
        }

        public Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            return inner.HandleResponse<T>(response);
        }

        public Task HandleResponse(HttpResponseMessage response)
        {
            return inner.HandleResponse(response);
        }

        private string GetBasicAuthString()
        {
            return $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{appKey}:{secret}"))}";
        }
    }
}
