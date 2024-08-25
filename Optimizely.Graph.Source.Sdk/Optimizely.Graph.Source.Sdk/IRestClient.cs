
namespace Optimizely.Graph.Source.Sdk
{
    /// <summary>
    /// The IRestClient interface describes the contract for a client capable of communicating 
    /// with REST services.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Sends the specified request and delivers a response.
        /// </summary>
        /// <param name="request">Request to be sent.</param>
        /// <returns>Response resulting from the request.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        /// <summary>
        /// Sends the specified request and delivers a response.
        /// </summary>
        /// <param name="request">Request to be sent.</param>
        /// <param name="cancellationToken">Cancellation token to signal that the operation should be interrupted.</param>
        /// <returns>Response resulting from the request.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        /// <summary>
        /// Handles a response from the REST service.
        /// </summary>
        /// <typeparam name="T">Type of content in response.</typeparam>
        /// <param name="response">Response message to be handled.</param>
        /// <returns>The response content.</returns>
        Task<T> HandleResponse<T>(HttpResponseMessage response);

        /// <summary>
        /// Handles a response from the REST service.
        /// </summary>
        /// <param name="response">Response message to be handled.</param>
        /// <returns>A task that executes the action.</returns>
        Task HandleResponse(HttpResponseMessage response);
    }
}
