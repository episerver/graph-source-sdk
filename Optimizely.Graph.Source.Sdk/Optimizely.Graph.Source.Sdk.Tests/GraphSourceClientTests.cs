using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GraphSourceClientTests
    {
        [TestMethod]
        public void Create_ReturnsConfiguredClient()
        {
            // Arrange & Act
            var client = GraphSourceClient.Create(new UriBuilder("https://test.url/").Uri, "app-key", "source", "secret");

            // Assert
            Assert.IsNotNull(client);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Create_ThrowsForInvalidAppKey(string appKey)
        {
            Assert.ThrowsException<ArgumentException>(() => GraphSourceClient.Create(new Uri("https://test.url/"), "source", appKey, "secret"));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Create_ThrowsForInvalidSecretKey(string secret)
        {
            Assert.ThrowsException<ArgumentException>(() => GraphSourceClient.Create(new Uri("https://test.url/"), "source", "app-key", secret));
        }
    }
}
