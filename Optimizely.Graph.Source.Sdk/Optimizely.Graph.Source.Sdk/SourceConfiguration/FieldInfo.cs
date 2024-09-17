namespace Optimizely.Graph.Source.Sdk.SourceConfiguration
{
    public class FieldInfo
    {
        public string Name { get; set; }

        public IndexingType IndexingType { get; set; }

        public Type MappedType { get; set; }

        public string MappedTypeName { get; set; }

        public override string ToString()
        {
            if (IndexingType == IndexingType.PropertyType)
            {
                return Name;
            }

            var value = $"{Name}$${MappedTypeName}";
            if (IndexingType == IndexingType.Searchable)
            {
                value += "___searchable";
            }
            else if (IndexingType == IndexingType.OnlyStored)
            {
                value += "___skip";
            }

            return value;
        }
    }
}
