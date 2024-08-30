// See https://aka.ms/new-console-template for more information

using Optimizely.Graph.Source.Sdk;
using Optimizely.Graph.Source.Sdk.Sample;
using Optimizely.Graph.Source.Sdk.SourceConfiguration;

//var client = GraphSourceClient.Create("https://cg.optimizely.com", "", "", "");
var client = GraphSourceClient.Create(
    new Uri("https://cg.optimizely.com"),
    "ed",
    "W0LCG2J0CTXtFnJGI0DMFGas1zLNPSRYU0jZJyu4uslPEYS4",
    "2PiblNXuFA7o7q3EG0csdLTBLe7rcX94GfecOxrbq6FMLXHezm/BQBOkmK6zP8WO"
);

client.AddLanguage("en");

client.ConfigureContentType<Cafe>()
    .Field(x => x.Name, IndexingType.Querable)
    .Field(x => x.Established, IndexingType.Searchable)
    .Field(x => x.Address, IndexingType.PropertyType)
    .Field(x => x.Menu, IndexingType.PropertyType);

client.ConfigurePropertyType<Location>()
    .Field(x => x.City, IndexingType.Querable)
    .Field(x => x.State, IndexingType.Querable)
    .Field(x => x.Zipcode, IndexingType.Searchable)
    .Field(x => x.Country, IndexingType.Searchable);

client.ConfigurePropertyType<Menu>()
    .Field(x => x.Beverages, IndexingType.PropertyType)
    .Field(x => x.Food, IndexingType.PropertyType);

client.ConfigurePropertyType<Beverage>()
    .Field(x => x.Name, IndexingType.Querable)
    .Field(x => x.Price, IndexingType.Querable)
    .Field(x => x.Sizes, IndexingType.Searchable);

client.ConfigurePropertyType<FoodItem>()
    .Field(x => x.Name, IndexingType.Querable)
    .Field(x => x.Price, IndexingType.Querable)
    .Field(x => x.IsAvaiable, IndexingType.Searchable);

await client.SaveTypesAsync();

var exampleDataInstance1 = new Cafe
{
    Name = "Optimizely's Awesome Cafe",
    Established = new DateTime(2024, 06, 12),
    Address = new Location
    {
        City = "New York",
        State = "NY",
        Zipcode = "10003",
        Country = "USA"
    },
    Menu = new Menu
    {
        Beverages = new List<Beverage>
        {
            new() {
                Name = "Espresso",
                Price = 4.99,
                Sizes = new[] { "S", "M" }
            },
            new() {
                Name = "Latte",
                Price = 5.99,
                Sizes = new[] { "M", "L" }
            },
            new() {
                Name = "Cappuccino",
                Price = 6.99,
                Sizes = new[] { "S", "M", "L" }
            }
        },
        Food = new List<FoodItem>
        {
            new() {
                Name = "Bagel",
                Price = 5.25,
                IsAvaiable = true
            },
            new() {
                Name = "Croissant",
                Price = 3.89,
                IsAvaiable = true
            },
            new() {
                Name = "Cinnamon Roll",
                Price = 4.99,
                IsAvaiable = false
            }
        }
    }
};

var exampleDataInstance2 = new Cafe
{
    Name = "Graph's Team Super Awesome Cafe",
    Established = new DateTime(2024, 08, 28),
    Address = new Location
    {
        City = "Stockholm",
        Country = "Sweden"
    },
    Menu = new Menu
    {
        Beverages = new List<Beverage>
        {
            new() {
                Name = "Espresso",
                Price = 2.99,
                Sizes = new[] { "S", "M", "L" }
            },
            new() {
                Name = "Tea",
                Price = 1.99,
                Sizes = new[] { "S", "M", "L" }
            }
        },
        Food = new List<FoodItem>
        {
            new() {
                Name = "Croissant",
                Price = 4.50,
                IsAvaiable = true
            },
            new() {
                Name = "Pretzel",
                Price = 5.99,
                IsAvaiable = true
            }
        }
    }
};

var exampleDataInstance3 = new Cafe
{
    Name = "Jake's Even More Super Awesome Cafe",
    Established = new DateTime(2024, 09, 13),
    Address = new Location
    {
        City = "Southington",
        State = "CT",
        Zipcode = "06489",
        Country = "USA"
    },
    Menu = new Menu
    {
        Beverages = new List<Beverage>
        {
            new() {
                Name = "Cortado",
                Price = 3.16,
                Sizes = new[] { "S" }
            },
            new() {
                Name = "Latte",
                Price = 4.55,
                Sizes = new[] { "M", "L" }
            },
            new() {
                Name = "Iced Latte",
                Price = 5.55,
                Sizes = new[] { "M", "L" }
            },
            new() {
                Name = "Tea",
                Price = 2.79,
                Sizes = new[] { "S", "M", "L" }
            },
            new() {
                Name = "Iced Tea",
                Price = 3.99,
                Sizes = new[] { "S", "M", "L" }
            }
        },
        Food = new List<FoodItem>
        {
            new() {
                Name = "Breakfast Wrap",
                Price = 4.80,
                IsAvaiable = true
            },
            new() {
                Name = "Pancakes",
                Price = 4.99,
                IsAvaiable = false
            },
            new() {
                Name = "Danish",
                Price = 1.99,
                IsAvaiable = true
            }
        }
    }
};

await client.SaveContentAsync(generateId: (x) => x.ToString(), exampleDataInstance1, exampleDataInstance2, exampleDataInstance3);

//await client.DeleteContentAsync();

Console.WriteLine("Hello, World!");