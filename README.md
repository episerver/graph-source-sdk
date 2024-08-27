# Content Graph .Net C# SDK

C# SDK for Content Graph Services api.

This SDK is a C# wrapper around the Optimizely Content Graph API.

## Accessing the Package

The package is published to the Episerver package repository as Optimizely.Graph.Source.Sdk

## Usage
### Set up your Content Graph Instance and Keypair

You will need an application key and secret key provided by your Turnstile credential to get started.
You can find more information [here]https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/introduction-optimizely-graph

### Using the SDK

You can use the client by calling `Create()` and providing your base url, Content Graph source, application key and secret, then calling one of the provided functions for synchronizing Content Types and Content Data.

```csharp
// Example of initializing client and configuring content types
var graphSourceClient = GraphSourceClient.Create(new UriBuilder("https://cg.optimizely.com").Uri, "source", "application-key", "secret");

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

repository.ConfigureContentType<ExampleClassObject>()
    .Field(x => x.FirstName, IndexingType.Searchable)
    .Field(x => x.LastName, IndexingType.Searchable)
    .Field(x => x.Age, IndexingType.Querable)
    .Field(x => x.SubType, IndexingType.PropertyType);

repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
    .Field(x => x.One, IndexingType.Searchable)
    .Field(x => x.Two, IndexingType.Querable);

var result = await graphSourceClient.SaveTypesAsync();
```

## Run Examples

In visual studio, set your startup project to the Optimizely.Graph.Source.Sdk.Sample project.

## Issues

Log issues directly into GitHub. Pull Requests will be created to resolve those issues as soon as they are triaged.
