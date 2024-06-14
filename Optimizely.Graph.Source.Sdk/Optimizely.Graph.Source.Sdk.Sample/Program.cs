// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk.Repository;
using Optimizely.Graph.Source.Sdk.Sample;

var repository = new DefaultGraphSourceRepository();

repository.Confiture<ExampleData>((x) =>
{
    x.Add(x => x.FirstName, Optimizely.Graph.Source.Sdk.Model.IndexingType.Searchable);
    x.Add(x => x.LastName, Optimizely.Graph.Source.Sdk.Model.IndexingType.Searchable);
    x.Add(x => x.Age, Optimizely.Graph.Source.Sdk.Model.IndexingType.Querable);
});
await repository.SaveTypeAsync<ExampleData>();

var exampleDataInstance = new ExampleData
{
    FirstName = "Jonas",
    LastName = "Bergqvist",
    Age = 43
};
await repository.SaveAsync((x) => $"{x.FirstName}_{x.LastName}_{x.Age}", exampleDataInstance);

Console.WriteLine("Hello, World!");
