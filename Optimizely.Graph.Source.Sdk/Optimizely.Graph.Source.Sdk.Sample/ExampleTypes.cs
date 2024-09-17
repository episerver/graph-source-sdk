namespace Optimizely.Graph.Source.Sdk.Sample.TypesExample
{
    public class ExampleTypes
    {
        public bool BoolType { get; set; }

        public IEnumerable<bool> BoolTypes { get; set; }

        public DateTime DateType { get; set; }

        public IEnumerable<DateTime> DateTypes { get; set; }

        public int IntType { get; set; }

        public IEnumerable<int> IntTypes { get; set; }

        public double DoubleType { get; set; }

        public IEnumerable<double> DoubleTypes { get; set; }

        public string StringType { get; set; }

        public IEnumerable<string> StringTypes { get; set; }

        public BaseType BaseType { get; set; }
    }

    public class BaseType
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public NestedType NestedType { get; set; }

        public IEnumerable<NestedType> NestedTypes { get; set; }
    }

    public class NestedType
    {
        public IEnumerable<string> NestedStrings { get; set; }
    }
}
