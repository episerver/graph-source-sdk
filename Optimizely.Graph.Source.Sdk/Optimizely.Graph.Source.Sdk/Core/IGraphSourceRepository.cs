namespace Optimizely.Graph.Source.Sdk.Core
{
    public interface IGraphSourceRepository
    {
        string AppKey { get; }

        string Secret { get; }

        string BaseUrl { get; }
    }
}
