using Moq;
using Optimizely.Graph.Source.Sdk.BasicAuth;
using Optimizely.Graph.Source.Sdk.RestClientHelpers;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests.BasicAuthTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BasicAuthClientTests
    {
        private readonly Mock<IRestClient> inner = new Mock<IRestClient>();
        private readonly string appKey = "app-key";
        private readonly string secret = "secret";
        private readonly BasicAuthClient client;

        public BasicAuthClientTests()
        {
            client = new BasicAuthClient(inner.Object, appKey, secret);
        }

        [TestMethod]
        public async Task SendRequest_WithoutCancellation_AddsAuthHeaderAndCallsInner()
        {
            // Arrange
            var message = new HttpRequestMessage();
            var responseMessage = Mock.Of<HttpResponseMessage>();
            inner.Setup(c => c.SendAsync(message)).ReturnsAsync(responseMessage);

            // Act
            var response = await client.SendAsync(message);

            // Assert
            inner.Verify(c => c.SendAsync(message));
            Assert.IsNotNull(message.Headers.Authorization);
        }

        [TestMethod]
        public async Task SendRequest_WithCancellation_AddsAuthHeaderAndCallsInner()
        {
            // Arrange
            var message = new HttpRequestMessage();
            var ct = new CancellationToken();
            var responseMessage = Mock.Of<HttpResponseMessage>();
            inner.Setup(c => c.SendAsync(message)).ReturnsAsync(responseMessage);

            // Act
            var response = await client.SendAsync(message, ct);

            // Assert
            inner.Verify(c => c.SendAsync(message, ct));
            Assert.IsNotNull(message.Headers.Authorization);
        }

        [TestMethod]
        public async Task HandleResponse_WithType_CallsInner()
        {
            // Arrange
            var message = Mock.Of<HttpResponseMessage>();

            // Act
            await client.HandleResponse<Type>(message);

            // Assert
            inner.Verify(c => c.HandleResponse<Type>(message));
        }

        [TestMethod]
        public async Task HandleResponse_WithoutType_CallsInner()
        {
            // Arrange
            var message = Mock.Of<HttpResponseMessage>();

            // Act
            await client.HandleResponse(message);

            // Assert
            inner.Verify(c => c.HandleResponse(message));
        }
    }
}
