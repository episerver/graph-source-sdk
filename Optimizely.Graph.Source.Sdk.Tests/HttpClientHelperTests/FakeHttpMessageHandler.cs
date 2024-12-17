using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests.HttpClientHelperTests
{
    [ExcludeFromCodeCoverage]
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new InvalidOperationException("This method was not mocked or its setup was not matched.");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
