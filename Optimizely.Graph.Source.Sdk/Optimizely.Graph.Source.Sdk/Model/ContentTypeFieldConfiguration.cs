namespace Optimizely.Graph.Source.Sdk.Model
{
    public class ContentTypeFieldConfiguration
    {
        public ContentTypeFieldConfiguration(Type type)
        {
            ContentTypeName = type.Name;
            //Fields = new Dictionary<string, IndexingType>();
            Fields = new List<FieldInfo>();
        }

        public string ContentTypeName { get; set; }

        //public IDictionary<string, IndexingType> Fields { get; private set; }
        public IList<FieldInfo> Fields { get; private set; }
    }
}
