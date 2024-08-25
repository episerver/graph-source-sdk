// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk;
using Optimizely.Graph.Source.Sdk.Models;
using Optimizely.Graph.Source.Sdk.Sample;

//var client = GraphSourceClient.Create("https://cg.optimizely.com", "", "", "");
var client = GraphSourceClient.Create(new UriBuilder("https://cg.optimizely.com").Uri, "ed", "W0LCG2J0CTXtFnJGI0DMFGas1zLNPSRYU0jZJyu4uslPEYS4", "2PiblNXuFA7o7q3EG0csdLTBLe7rcX94GfecOxrbq6FMLXHezm/BQBOkmK6zP8WO");

client.AddLanguage("en");

client.ConfigureContentType<PersonDetails>()
    .Field(x => x.FirstName, IndexingType.Searchable)
    .Field(x => x.LastName, IndexingType.Searchable)
    .Field(x => x.Email, IndexingType.Searchable)
    .Field(x => x.Age, IndexingType.Querable)
    .Field(x => x.BirthDate, IndexingType.PropertyType)
    .Field(x => x.Location, IndexingType.PropertyType);

client.ConfigurePropertyType<BirthDate>()
    .Field(x => x.Month, IndexingType.Searchable)
    .Field(x => x.Day, IndexingType.Querable)
    .Field(x => x.Year, IndexingType.Searchable);

client.ConfigurePropertyType<Location>()
    .Field(x => x.City, IndexingType.Querable)
    .Field(x => x.State, IndexingType.Querable)
    .Field(x => x.Country, IndexingType.Searchable)
    .Field(x => x.Zip, IndexingType.Searchable);

await client.SaveTypesAsync();

var exampleDataInstance1 = new PersonDetails
{
    FirstName = "Jake",
    LastName = "Minard",
    Email = "jake.minard@opti.com",
    Age = 29,
    BirthDate = new BirthDate
    {
        Month = 06,
        Day = 12,
        Year = 1995
    },
    Location = new Location
    {
        City = "Southington",
        State = "CT",
        Country = "USA",
        Zip = "06489"
    }
};
var exampleDataInstance2 = new PersonDetails
{
    FirstName = "Victoria",
    LastName = "Minard",
    Email = "victoria@gmail.com",
    Age = 28,
    BirthDate = new BirthDate
    {
        Month = 09,
        Day = 14,
        Year = 1995
    },
    Location = new Location
    {
        City = "Southington",
        State = "CT",
        Country = "USA",
        Zip = "06489"
    }
};

var exampleDataInstance3 = new PersonDetails
{
    FirstName = "Grace",
    LastName = "Minard",
    Email = "grace@gmail.com",
    Age = 8,
    BirthDate = new BirthDate
    {
        Month = 03,
        Day = 28,
        Year = 2016
    }
};
await client.SaveContentAsync(generateId: (x) => x.ToString(), exampleDataInstance3);

Console.WriteLine("Hello, World!");