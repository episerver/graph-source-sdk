// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk.Repository;
using Optimizely.Graph.Source.Sdk.Sample;

var repository = new DefaultGraphSourceRepository("https://cg.optimizely.com", "", "");

repository.Confiture<ExampleData>()
    .Field(x => x.FirstName, Optimizely.Graph.Source.Sdk.Model.IndexingType.Searchable)
    .Field(x => x.LastName, Optimizely.Graph.Source.Sdk.Model.IndexingType.Searchable)
    .Field(x => x.Age, Optimizely.Graph.Source.Sdk.Model.IndexingType.Querable);

await repository.SaveTypeAsync<ExampleData>();

var exampleDataInstance = new ExampleData
{
    FirstName = "Jonas",
    LastName = "Bergqvist",
    Age = 43
};
await repository.SaveAsync((x) => x.ToString(), exampleDataInstance);

Console.WriteLine("Hello, World!");