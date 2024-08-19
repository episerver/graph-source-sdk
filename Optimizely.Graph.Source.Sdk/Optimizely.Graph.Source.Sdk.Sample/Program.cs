// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk;
using Optimizely.Graph.Source.Sdk.Models;
using Optimizely.Graph.Source.Sdk.Sample;

//var client = GraphSourceClient.Create("https://cg.optimizely.com", "", "", "");
var client = GraphSourceClient.Create("https://cg.optimizely.com", "ed", "W0LCG2J0CTXtFnJGI0DMFGas1zLNPSRYU0jZJyu4uslPEYS4", "2PiblNXuFA7o7q3EG0csdLTBLe7rcX94GfecOxrbq6FMLXHezm/BQBOkmK6zP8WO");

client.AddLanguage("en");

client.ConfigureContentType<ExampleData>()
    .Field(x => x.FirstName, IndexingType.Searchable)
    .Field(x => x.LastName, IndexingType.Searchable)
    .Field(x => x.Age, IndexingType.Querable)
    .Field(x => x.SubType, IndexingType.PropertyType);

client.ConfigurePropertyType<SubType1>()
    .Field(x => x.One, IndexingType.Searchable)
    .Field(x => x.Two, IndexingType.Querable)
    .Field(x => x.AnotherType, IndexingType.PropertyType);

client.ConfigurePropertyType<SubType2>()
    .Field(x => x.Four, IndexingType.Querable)
    .Field(x => x.Five, IndexingType.Querable);

await client.SaveTypesAsync();

var exampleDataInstance1 = new ExampleData
{
    FirstName = "Testing1",
    LastName = "1",
    Age = 100,
    SubType = new SubType1
    { 
        One = "This is a test",
        Two = 13,
        AnotherType = new SubType2
        {
            Four = "Who knows",
            Five = "Somewhere"
        }
    }
};
var exampleDataInstance2 = new ExampleData
{
    FirstName = "Testing2",
    LastName = "2",
    Age = 99,
    SubType = new SubType1
    {
        One = "This is also a test",
        Two = 14,
        AnotherType = new SubType2
        {
            Four = "Not sure",
            Five = "Yeah"
        }
    }
};
await client.SaveContentAsync(generateId: (x) => x.ToString(), exampleDataInstance1, exampleDataInstance2);

Console.WriteLine("Hello, World!");