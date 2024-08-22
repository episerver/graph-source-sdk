using Moq;
using Optimizely.Graph.Source.Sdk.ContentGraph;
using Optimizely.Graph.Source.Sdk.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests.ClientTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GraphSourceClientTests
    {
        private GraphSourceClient client;
        private Mock<IContentGraphClientFactory> mockGraphClientFactory;
        private Mock<IContentGraphClient> mockGraphClient;
        private Mock<IGraphSourceRepository> mockGraphRepository;

        public GraphSourceClientTests()
        {
            client = GraphSourceClient.Create("https://test.url/", "application-key", "source", "application-secret");
            mockGraphClientFactory = new Mock<IContentGraphClientFactory>();
            mockGraphClient = new Mock<IContentGraphClient>();
            mockGraphRepository = new Mock<IGraphSourceRepository>();
        }

        
    }
}
