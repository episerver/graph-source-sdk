namespace Optimizely.Graph.Source.Sdk.Model
{
    public class FieldInfo
    {
        public string Name { get; set; }

        public IndexingType IndexingType { get; set; }

        public   string MappedType { get; set; }

        public override string ToString()
        {
            var value = $"{Name}$${MappedType}";
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
