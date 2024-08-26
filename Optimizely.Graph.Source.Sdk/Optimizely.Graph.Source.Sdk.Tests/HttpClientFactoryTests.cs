using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class HttpClientFactoryTests
    {
        [TestMethod]
        public void Create_ThrowsForNullServiceBaseAddress()
        {
            // Arrange
            var factory = new HttpClientFactory();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null));
        }

        [TestMethod]
        public void Create_CreatesHttpClient()
        {
            // Arrange
            var factory = new HttpClientFactory();
            var uri = new Uri("https://service.base.address");

            // Act
            var client = factory.Create(uri);

            // Assert
            Assert.IsNotNull(client);
            Assert.AreEqual(uri, client.BaseAddress);
        }
    }
}
