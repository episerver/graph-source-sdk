namespace Optimizely.Graph.Source.Sdk.Tests.ExampleObjects
{
    public class ExampleClassObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public SubType1 SubType { get; set; }

        public class SubType1
        {
            public string One { get; set; }

            public int Two { get; set; }
        }
    }
}
