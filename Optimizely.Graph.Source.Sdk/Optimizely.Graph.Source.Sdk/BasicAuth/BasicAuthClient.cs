using System.Text;
using Optimizely.Graph.Source.Sdk.RestClientHelpers;

namespace Optimizely.Graph.Source.Sdk.BasicAuth
{
    /// <summary>
    /// The BasicAuthClient class is a component capable of managing Basic Authorization within REST services.
    /// </summary>
    public class BasicAuthClient : IRestClient
    {
        private readonly IRestClient inner;
        public readonly string appKey;
        public readonly string secret;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inner">Inner Rest client.</param>
        /// <param name="appKey">Application key.</param>
        /// <param name="secret">Application secret.</param>
        public BasicAuthClient(IRestClient inner, string appKey, string secret)
        {
            this.inner = inner;
            this.appKey = appKey;
            this.secret = secret;
        }

        /// <summary>
        /// Sends http request to REST services with basic authorization.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", GetBasicAuthString());
            return await inner.SendAsync(request);
        }

        /// <summary>
        /// Sends http request to REST services with basic authorization and cancellation token.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", GetBasicAuthString());
            return await inner.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Handles http response message with generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            return inner.HandleResponse<T>(response);
        }

        /// <summary>
        /// Handles http response message.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
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
