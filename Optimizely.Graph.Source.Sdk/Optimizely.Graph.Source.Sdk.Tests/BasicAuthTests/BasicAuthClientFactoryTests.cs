using Moq;
using Optimizely.Graph.Source.Sdk.BasicAuth;
using Optimizely.Graph.Source.Sdk.RestClientHelpers;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests.BasicAuthTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BasicAuthClientFactoryTests
    {
        private Mock<IRestClient> mockRestClient;

        public BasicAuthClientFactoryTests()
        {
            mockRestClient = new Mock<IRestClient>();
        }

        [TestMethod]
        public async Task Create_CreatesHttpClient()
        {
            // Arrange
            var clientFactory = new BasicAuthClientFactory("app-key", "secret");

            // Act
            var client = clientFactory.Create(mockRestClient.Object);

            // Assert
            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void Constructor_WithEmptyAppKey_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BasicAuthClientFactory(null, "secret"));
        }

        [TestMethod]
        public void Constructor_WithEmptySecret_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BasicAuthClientFactory("app-key", null));
        }
    }
}