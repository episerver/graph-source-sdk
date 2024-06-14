using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Model
{
    public class ContentTypeFieldConfiguration
    {
        public ContentTypeFieldConfiguration(Type type)
        {
            ContentTypeName = type.Name;
            Fields = new Dictionary<string, IndexingType>();
        }

        public string ContentTypeName { get; set; }

        public IDictionary<string, IndexingType> Fields { get; private set; }
    }
}
