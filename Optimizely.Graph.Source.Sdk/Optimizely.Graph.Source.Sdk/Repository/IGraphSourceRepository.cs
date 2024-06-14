namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceRepository
    {
        string AppKey { get; }

        string Secret {  get; }

        string BaseUrl { get; }
    }
}
