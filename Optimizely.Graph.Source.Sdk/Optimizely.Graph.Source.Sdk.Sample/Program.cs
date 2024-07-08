// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk.Model;
using Optimizely.Graph.Source.Sdk.Repository;
using Optimizely.Graph.Source.Sdk.Sample;

var repository = new DefaultGraphSourceRepository("https://cg.optimizely.com", "", "", "");

repository.AddLanguage("en");

repository.ConfigureContentType<ExampleData>()
    .Field(x => x.FirstName, IndexingType.Searchable)
    .Field(x => x.LastName, IndexingType.Searchable)
    .Field(x => x.Age, IndexingType.Querable)
    .Field(x => x.SubType, IndexingType.PropertyType);

repository.ConfigurePropertyType<SubType1>()
    .Field(x => x.One, IndexingType.Searchable)
    .Field(x => x.Two, IndexingType.OnlyStored);

await repository.SaveTypesAsync();

var exampleDataInstance1 = new ExampleData
{
    FirstName = "Jonas",
    LastName = "Bergqvist",
    Age = 43,
    SubType = new SubType1
    { 
        One = "Fagerstrand",
        Two = 125
    }
};
var exampleDataInstance2 = new ExampleData
{
    FirstName = "William",
    LastName = "Bergqvist",
    Age = 14,
    SubType = new SubType1
    {
        One = "Fagerstrand",
        Two = 125
    }
};
await repository.SaveAsync(generateId: (x) => x.ToString(), exampleDataInstance1, exampleDataInstance2);

Console.WriteLine("Hello, World!");