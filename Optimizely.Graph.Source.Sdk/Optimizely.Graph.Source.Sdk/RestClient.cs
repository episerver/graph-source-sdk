using Newtonsoft.Json;
using Optimizely.Graph.Source.Sdk.Core.Exceptions;
using System.Net;

namespace Optimizely.Graph.Source.Sdk
{
    /// <summary>
    /// The RestClient class describes a client capable of communicating with REST services.
    /// </summary>
    public class RestClient : IRestClient
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClientFactory">Factory for constructing HTTP clients targeting REST services.</param>
        /// <param name="endpoint">Endpoint for target service.</param>
        /// <param name="timeoutInMilliseconds">Timeout for each response made by the rest client.</param>
        public RestClient(IHttpClientFactory httpClientFactory, Uri endpoint)
        {
            httpClient = httpClientFactory.Create(endpoint);
        }

        /// <summary>
        /// Sends the specified request and delivers a response.
        /// </summary>
        /// <param name="requestMessage">Request to be sent.</param>
        /// <returns>Response resulting from the request.</returns>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            return SendAsync(requestMessage, CancellationToken.None);
        }

        /// <summary>
        /// Sends the specified request and delivers a response.
        /// </summary>
        /// <param name="requestMessage">Request to be sent.</param>
        /// <param name="cancellationToken">Cancellation token to signal that the operation should be interrupted.</param>
        /// <returns>Response resulting from the request.</returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            HttpResponseMessage responseMessage;

            try
            {
                responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                throw new Core.Exceptions.TimeoutException();
            }
            catch (Exception ex)
            {
                throw new CommunicationException("An unexpected communication error has occurred.", ex);
            }

            return responseMessage;
        }

        /// <summary>
        /// Handles a response from the REST service.
        /// </summary>
        /// <typeparam name="T">Type of content in response.</typeparam>
        /// <param name="responseMessage">Response message to be handled.</param>
        /// <returns>The response content.</returns>
        public async Task<T> HandleResponse<T>(HttpResponseMessage responseMessage)
        {
            T result = default;

            if (responseMessage.IsSuccessStatusCode)
            {
                result = await HandleSuccessfulResponseAsync<T>(responseMessage).ConfigureAwait(false);
            }
            else
            {
                await HandleUnsuccessfulResponseAsync(responseMessage).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// Handles a response from the REST service.
        /// </summary>
        /// <param name="responseMessage">Response message to be handled.</param>
        /// <returns>A task that executes the action.</returns>
        public async Task HandleResponse(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                await HandleUnsuccessfulResponseAsync(responseMessage).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles a response resulting from a successful request.
        /// </summary>
        /// <typeparam name="T">Type of content in response.</typeparam>
        /// <param name="responseMessage">Response message to be handled.</param>
        /// <returns>The content of the HTTP response.</returns>
        private static async Task<T> HandleSuccessfulResponseAsync<T>(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        /// <summary>
        /// Handles a response resulting from an unsuccessful request.
        /// </summary>
        /// <param name="responseMessage">The response message to be handled.</param>
        /// <returns>A task that executes the action.</returns>
        private static async Task HandleUnsuccessfulResponseAsync(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var validationResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new ValidationException(validationResult);
                case HttpStatusCode.InternalServerError:
                    throw new ServiceErrorException(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));
                case HttpStatusCode.NotFound:
                    throw new NotFoundException();
                case HttpStatusCode.Unauthorized:
                    throw new NotAuthorizedException();
                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException();
                case HttpStatusCode.RequestTimeout:
                    throw new Core.Exceptions.TimeoutException();
                default:
                    throw new ServiceErrorException();
            }
        }
    }
}
