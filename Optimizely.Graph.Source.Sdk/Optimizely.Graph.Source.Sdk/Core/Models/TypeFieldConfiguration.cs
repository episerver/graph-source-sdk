namespace Optimizely.Graph.Source.Sdk.Core.Models
{
    public enum ConfigurationType
    {
        ContentType = 0,
        PropertyType = 1
    }

    public class TypeFieldConfiguration
    {
        public TypeFieldConfiguration(Type type, ConfigurationType configurationType)
        {
            TypeName = type.Name;
            Fields = new List<FieldInfo>();
            ConfigurationType = configurationType;
        }

        public ConfigurationType ConfigurationType { get; private set; }

        public string TypeName { get; set; }

        public IList<FieldInfo> Fields { get; private set; }
    }
}
