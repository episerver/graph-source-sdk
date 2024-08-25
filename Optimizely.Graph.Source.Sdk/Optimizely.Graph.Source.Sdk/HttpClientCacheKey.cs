using System.Security.Cryptography;
using System.Text;

namespace Optimizely.Graph.Source.Sdk
{
    /// <summary>
    /// The HttpClientCacheKey class represents a unique key applied in the
    /// caching of HttpClient instances.
    /// </summary>
    public class HttpClientCacheKey
    {
        private readonly string hash;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The value identifying the cache key.</param>
        public HttpClientCacheKey(string value)
        {
            hash = GenerateHash(value);
        }

        /// <summary>
        /// Returns the string representation of this HttpClientCacheKey.
        /// </summary>
        /// <returns>A string representation of this key.</returns>
        public override string ToString()
        {
            return hash;
        }

        /// <summary>
        /// Generates a unique hash from the components of this key.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>The unique hash for the key.</returns>
        protected virtual string GenerateHash(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                var contentAsBytes = Encoding.UTF8.GetBytes(value);
                var hashAsBytes = md5.ComputeHash(contentAsBytes);
                return Convert.ToBase64String(hashAsBytes);
            }
        }
    }
}
