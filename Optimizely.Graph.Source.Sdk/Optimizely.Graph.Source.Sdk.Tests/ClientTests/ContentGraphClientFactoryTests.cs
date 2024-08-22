using Optimizely.Graph.Source.Sdk.ContentGraph;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests.ClientTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContentGraphClientFactoryTests
    {
        [TestMethod]
        public async Task Create_CreatesHttpClient()
        {
            // Arrange
            var clientFactory = new ContentGraphClientFactory("https://test.url/", "application-key", "source", "application-secret");

            // Act
            var client = clientFactory.Create();

            // Assert
            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void Constructor_WithNullBaseUrl_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ContentGraphClientFactory(null, "application-key", "source", "application-secret"));
        }

        [TestMethod]
        public void Constructor_WithEmptyAppKey_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ContentGraphClientFactory("https://test.url/", null, "source", "application-secret"));
        }

        [TestMethod]
        public void Constructor_WithEmptySource_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ContentGraphClientFactory("https://test.url/", "application-key", null, "application-secret"));
        }

        [TestMethod]
        public void Constructor_WithEmptySecret_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ContentGraphClientFactory("https://test.url/", "application-key", "source", null));
        }
    }
}