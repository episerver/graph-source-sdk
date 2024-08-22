namespace Optimizely.Graph.Source.Sdk.Sample
{
    public class ExampleData
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public SubType1 SubType { get; set; }

        public override string ToString()
        {
            return $"{FirstName}_{LastName}_{Age}";
        }
    }

    public class SubType1
    {
        public string One { get; set; }

        public int Two { get; set; }

        public SubType2 AnotherType { get; set; }
    }

    public class SubType2
    {
        public string Four { get; set; }

        public string Five { get; set; }

        public string Six { get; set; }
    }
}
