using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.BasicAuth
{
    public class BasicAuthClientFactory : IBasicAuthClientFactory
    {
        protected string appKey;
        protected string secret;

        public BasicAuthClientFactory(string appKey, string secret)
        {
            this.appKey = appKey ?? throw new ArgumentNullException(nameof(appKey));
            this.secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        public IRestClient Create(IRestClient inner)
        {
            return new BasicAuthClient(inner, appKey, secret);
        }
    }
}
