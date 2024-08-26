using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CachingHttpClientFactoryTests
    {
        [TestMethod]
        public void Create_ReturnsExpectedClient()
        {
            // Arrange
            var expectedUri = new Uri("http://localhost/api/resources/");
            var expectedHttpClient = Mock.Of<HttpClient>();

            var mockInnerFactory = new Mock<IHttpClientFactory>();
            mockInnerFactory.Setup(f => f.Create(It.Is<Uri>(actualUri => expectedUri == actualUri))).Returns(expectedHttpClient);

            var factory = new CachingHttpClientFactory(mockInnerFactory.Object, (uri) => new AuthenticatingHttpClientCacheKey(uri, "app-key", "secret"));

            // Act
            var actualHttpClient = factory.Create(expectedUri);

            // Assert
            mockInnerFactory.VerifyAll();
            Assert.AreEqual(expectedHttpClient, actualHttpClient);
        }

        [TestMethod]
        public void Create_ReturnsExistingClientUponRepeatRequests()
        {
            // Arrange
            var expectedHttpClient = Mock.Of<HttpClient>();
            var expectedHttpClient2 = Mock.Of<HttpClient>();

            var mockInnerFactory = new Mock<IHttpClientFactory>();
            mockInnerFactory.Setup(f => f.Create(It.IsAny<Uri>())).Returns(expectedHttpClient);

            var mockInnerFactory2 = new Mock<IHttpClientFactory>();
            mockInnerFactory2.Setup(f => f.Create(It.IsAny<Uri>())).Returns(expectedHttpClient2);

            var mockInnerFactory3 = new Mock<IHttpClientFactory>();
            mockInnerFactory3.Setup(f => f.Create(It.IsAny<Uri>())).Throws(new Exception("Create should never be invoked on this factory."));

            // Act
            var factory1 = new CachingHttpClientFactory(mockInnerFactory.Object, (uri) => new AuthenticatingHttpClientCacheKey(uri, "appkey", "secretkey"));
            var client1 = factory1.Create(new Uri("http://localhost/api/expected"));

            var factory2 = new CachingHttpClientFactory(mockInnerFactory2.Object, (uri) => new AuthenticatingHttpClientCacheKey(uri, "appkey", "secretkey"));
            var client2 = factory2.Create(new Uri("http://localhost/api/expected2"));

            var factory3 = new CachingHttpClientFactory(mockInnerFactory3.Object, (uri) => new AuthenticatingHttpClientCacheKey(uri, "appkey", "secretkey"));
            var client3 = factory3.Create(new Uri("http://localhost/api/expected"));

            // Assert
            Assert.AreEqual(client1, client3);
        }
    }
}
