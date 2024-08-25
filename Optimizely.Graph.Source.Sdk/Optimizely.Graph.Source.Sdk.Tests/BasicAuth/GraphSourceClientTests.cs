using Moq;
using Optimizely.Graph.Source.Sdk.BasicAuth;
using Optimizely.Graph.Source.Sdk.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Optimizely.Graph.Source.Sdk.Tests.BasicAuth
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
            //client = GraphSourceClient.Create("https://test.url/", "application-key", "source", "application-secret");
            mockGraphClientFactory = new Mock<IBasicAuthClientFactory>();
            mockGraphRepository = new Mock<IGraphSourceRepository>();
        }


    }
}
