namespace Optimizely.Graph.Source.Sdk.HttpClientHelpers
{
    /// <summary>
    /// The HttpClientFactory class constructs instances of
    /// HttpClient, which has been minimally configured to successfully
    /// communicate with a web service.
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Creates an instance of HttpClient.
        /// </summary>
        /// <param name="serviceBaseAddress">Base address for target service.</param>
        /// <returns>A new instance of HttpClient.</returns>
        public HttpClient Create(Uri serviceBaseAddress)
        {
            if (serviceBaseAddress == null)
            {
                throw new ArgumentNullException(nameof(serviceBaseAddress));
            }

            var baseHandler = CreateBaseHandler(serviceBaseAddress);
            var client = new HttpClient(baseHandler)
            {
                BaseAddress = serviceBaseAddress
            };

            return client;
        }

        /// <summary>
        /// Creates the base handler responsible for processing requests and responses.
        /// </summary>
        /// <param name="serviceBaseAddress">Base address of the target service.</param>
        /// <returns>The handler for HTTP communication.</returns>
        private HttpClientHandler CreateBaseHandler(Uri serviceBaseAddress)
        {
            var baseHandler = new HttpClientHandler();

            if (IsTrustedHost(serviceBaseAddress))
            {
                baseHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, certificateErrors) => true;
            }

            return baseHandler;
        }

        /// <summary>
        /// Determines whether or not the host for a request should be allowed regardless
        /// of the quality of its SSL certificates.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <remarks>True, if the host is trusted, false otherwise.</remarks>
        protected virtual bool IsTrustedHost(Uri requestUri)
        {
            return requestUri.IsLoopback;
        }
    }
}