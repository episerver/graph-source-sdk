using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using TimeoutException = Optimizely.Graph.Source.Sdk.Core.Exceptions.TimeoutException;
using Optimizely.Graph.Source.Sdk.Core.Exceptions;

namespace Optimizely.Graph.Source.Sdk.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RestClientTests
    {
        private readonly Mock<FakeHttpMessageHandler> mockHttpMessageHandler;
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        private readonly RestClient client;

        public RestClientTests()
        {
            mockHttpMessageHandler = new Mock<FakeHttpMessageHandler>() { CallBase = true };
            mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockHttpClientFactory.Setup(cf => cf.Create(It.IsAny<Uri>())).Returns(
                new HttpClient(mockHttpMessageHandler.Object)
                {
                    BaseAddress = new Uri("https://optimizely.net/")
                }
            );

            client = new RestClient(mockHttpClientFactory.Object, new Uri("https://optimizely.net/"));
        }

        [TestMethod]
        public void Ctor_CreatesHttpClient()
        {
            // Arrange
            var uri = new Uri("https://local.resource.net");

            // Act
            new RestClient(mockHttpClientFactory.Object, uri);

            // Assert
            mockHttpClientFactory.Verify(x => x.Create(uri));
        }

        [TestMethod]
        public async Task SendAsync_ReturnsARequest()
        {
            // Arrange
            var expected = BuildResponse();
            HttpRequestMessage requestMessage = null;

            var response = CreateResponse(HttpStatusCode.OK, expected);
            var request = GetRequest();

            mockHttpMessageHandler.Setup(handler => handler.Send(It.IsAny<HttpRequestMessage>()))
                                  .Callback((HttpRequestMessage actualRequestMessage) => requestMessage = actualRequestMessage)
                                  .Returns(CreateResponse(HttpStatusCode.OK, expected));

            // Act
            var responseMessage = await client.SendAsync(request);

            // Assert
            Assert.IsTrue(Compare(request, requestMessage));
        }

        [TestMethod]
        public async Task SendAsync_ReturnsARequest_WithCancellationToken()
        {
            // Arrange
            var expected = BuildResponse();
            HttpRequestMessage requestMessage = null;

            var response = CreateResponse(HttpStatusCode.OK, expected);
            var request = GetRequest();

            mockHttpMessageHandler.Setup(handler => handler.Send(It.IsAny<HttpRequestMessage>()))
                                  .Callback((HttpRequestMessage actualRequestMessage) => requestMessage = actualRequestMessage)
                                  .Returns(CreateResponse(HttpStatusCode.OK, expected));

            // Act
            var responseMessage = await client.SendAsync(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(Compare(request, requestMessage));
        }

        [TestMethod]
        public async Task SendAsync_WithCommunicationError_ThrowsCommunicationException()
        {
            // Arrange
            var response = CreateResponse(HttpStatusCode.Unauthorized);
            var request = GetRequest();

            HttpRequestMessage requestMessage = null;
            mockHttpMessageHandler.Setup(handler => handler.Send(It.IsAny<HttpRequestMessage>()))
                                  .Callback((HttpRequestMessage actualRequestMessage) => requestMessage = actualRequestMessage)
                                  .Throws<Exception>();

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<CommunicationException>(
                () => client.SendAsync(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task HandleResponse_BadRequest_ThrowsValidationException()
        {
            // Arrange
            var errorResponse = "empty response";
            var response = CreateResponse(HttpStatusCode.BadRequest, errorResponse);

            // Act & Assert
            var actual = await Assert.ThrowsExceptionAsync<ValidationException>(
                () => client.HandleResponse(response));
        }

        [TestMethod]
        public async Task HandleResponse_NotFound_ThrowsNotFoundException()
        {
            // Arrange
            var response = CreateResponse(HttpStatusCode.NotFound);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(
                        () => client.HandleResponse<string>(response));
        }

        [TestMethod]
        public async Task HandleResponse_WithInternalServerError_ThrowsServiceErrorException()
        {
            // Arrange
            var response = CreateResponse(HttpStatusCode.InternalServerError);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorException>(
                        () => client.HandleResponse<string>(response));
        }

        [TestMethod]
        public async Task HandleResponse_WithTimeout_ThrowsTimeoutException()
        {
            // Arrange
            var response = CreateResponse(HttpStatusCode.RequestTimeout);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<TimeoutException>(
                        () => client.HandleResponse<string>(response));
        }

        [TestMethod]
        public async Task HandleResponse_WithNotAuthorized_ThrowsNotAuthorizedException()
        {
            // Arrange
            var response = CreateResponse(HttpStatusCode.Unauthorized);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NotAuthorizedException>(
                        () => client.HandleResponse<string>(response));
        }

        [TestMethod]
        public async Task HandleResponse_ThrowsServiceErrorException_ByDefault()
        {
            // Arrange
            var response = CreateResponse(HttpStatusCode.Gone);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorException>(
                        () => client.HandleResponse<string>(response));
        }

        #region Private
        private HttpRequestMessage GetRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, "/api/endpoint?somevalue=something");
        }

        private bool Compare(HttpRequestMessage expected, HttpRequestMessage actual)
        {
            var method = expected.Method.Method == actual.Method.Method;
            var path = expected.RequestUri.ToString() == actual.RequestUri.ToString();
            return method && path;
        }

        private static string BuildResponse()
        {
            return "Test Response";
        }

        private HttpResponseMessage CreateResponse(HttpStatusCode statusCode)
        {
            return new HttpResponseMessage(statusCode);
        }

        private HttpResponseMessage CreateResponse<T>(HttpStatusCode statusCode, T content)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = JsonContent.Create(content)
            };
        }
        #endregion  
    }
}
