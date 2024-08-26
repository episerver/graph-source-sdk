using Moq;
using Optimizely.Graph.Source.Sdk.BasicAuth;
using Optimizely.Graph.Source.Sdk.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GraphSourceClientTests
    {
        private GraphSourceClient client;
        private Mock<IBasicAuthClientFactory> mockGraphClientFactory;
        private Mock<IGraphSourceRepository> mockGraphRepository;

        public GraphSourceClientTests()
        {
            client = GraphSourceClient.Create(new UriBuilder("https://test.url/").Uri, "application-key", "source", "application-secret");
            mockGraphClientFactory = new Mock<IBasicAuthClientFactory>();
            mockGraphRepository = new Mock<IGraphSourceRepository>();
        }
    }
}
