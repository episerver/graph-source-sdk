﻿// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk.Model;
using Optimizely.Graph.Source.Sdk.Repository;
using Optimizely.Graph.Source.Sdk.Sample;

var repository = new DefaultGraphSourceRepository("https://cg.optimizely.com", "", "", "");

repository.AddLanguage("en");
repository.Configure<ExampleData>()
    .Field(x => x.FirstName, IndexingType.Searchable)
    .Field(x => x.LastName, IndexingType.Searchable)
    .Field(x => x.Age, IndexingType.Querable);

await repository.SaveTypesAsync();

var exampleDataInstance = new ExampleData
{
    FirstName = "Jonas",
    LastName = "Bergqvist",
    Age = 43
};
await repository.SaveAsync(generateId: (x) => x.ToString(), exampleDataInstance);

Console.WriteLine("Hello, World!");