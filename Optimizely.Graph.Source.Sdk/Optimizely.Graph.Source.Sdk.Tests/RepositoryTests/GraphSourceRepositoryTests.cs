using Moq;
using System.Diagnostics.CodeAnalysis;
using Optimizely.Graph.Source.Sdk.Repositories;
using Optimizely.Graph.Source.Sdk.JsonConverters;
using System.Text.Json;
using System.Net;
using System.Text;
using Optimizely.Graph.Source.Sdk.RestClientHelpers;
using Optimizely.Graph.Source.Sdk.SourceConfiguration;
using Optimizely.Graph.Source.Sdk.Tests.ExampleObjects;
using System.Text.RegularExpressions;

namespace Optimizely.Graph.Source.Sdk.Tests.RepositoryTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GraphSourceRepositoryTests
    {
        private GraphSourceRepository repository;
        private Mock<IRestClient> mockRestClient;
        private readonly string source = "source";

        public GraphSourceRepositoryTests()
        {
            mockRestClient = new Mock<IRestClient>();
            repository = new GraphSourceRepository(mockRestClient.Object, source);
        }

        [TestInitialize]
        public void TestInit()
        {
            SourceConfigurationModel.Reset();
        }

        [TestMethod]
        public void Constructor_WithEmptySource_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new GraphSourceRepository(mockRestClient.Object, null));
        }

        [TestMethod]
        public void AddLanguage_SetsExpectedValue_InSourceConfigurationModel()
        {
            // Arrange
            var language = "en";

            // Act
            repository.AddLanguage(language);

            // Assert
            var actualLanguages = SourceConfigurationModel.GetLanguages();

            Assert.AreEqual(1, actualLanguages.Count());
            Assert.IsTrue(actualLanguages.Contains(language));
        }

        [TestMethod]
        public void ConfigureContentType_SetsExpectedContentTypes_InSourceConfigurationModel()
        {
            // Arrange & Act
            repository.ConfigureContentType<ExampleClassObject>()
                .Field(x => x.FirstName, IndexingType.Searchable)
                .Field(x => x.LastName, IndexingType.Searchable)
                .Field(x => x.Age, IndexingType.Queryable)
                .Field(x => x.SubType, IndexingType.PropertyType);


            // Assert
            var contentTypes = SourceConfigurationModel.GetContentTypeFieldConfiguration();

            Assert.AreEqual(contentTypes.First().ConfigurationType, ConfigurationType.ContentType);
            Assert.AreEqual(contentTypes.First().TypeName, nameof(ExampleClassObject));
            Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "FirstName" && x.IndexingType == IndexingType.Searchable));
            Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "LastName" && x.IndexingType == IndexingType.Searchable));
            Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "Age" && x.IndexingType == IndexingType.Queryable));
            Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "SubType" && x.IndexingType == IndexingType.PropertyType));
            Assert.AreEqual(4, contentTypes.First().Fields.Count);
        }

        [TestMethod]
        public void ConfigurePropertyType_SetsExpectedPropertyTypes_InSourceConfigurationModel()
        {
            // Arrange & Act
            repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
                .Field(x => x.One, IndexingType.Searchable)
                .Field(x => x.Two, IndexingType.Queryable);

            // Assert
            var propertyTypes = SourceConfigurationModel.GetPropertyTypeFieldConfiguration();

            Assert.AreEqual(propertyTypes.First().ConfigurationType, ConfigurationType.PropertyType);
            Assert.AreEqual(propertyTypes.First().TypeName, "SubType1");
            Assert.IsTrue(propertyTypes.First().Fields.Any(x => x.Name == "One" && x.IndexingType == IndexingType.Searchable));
            Assert.IsTrue(propertyTypes.First().Fields.Any(x => x.Name == "Two" && x.IndexingType == IndexingType.Queryable));
            Assert.AreEqual(2, propertyTypes.First().Fields.Count);
        }

        [TestMethod]
        public async Task SaveTypesAsync_BuildsExpectedJsonString_AndCallsGraphClient()
        {
            // Arrange
            repository.ConfigureContentType<ExampleClassObject>()
                .Field(x => x.FirstName, IndexingType.Searchable)
                .Field(x => x.LastName, IndexingType.Searchable)
                .Field(x => x.Age, IndexingType.Queryable);

            var expectedJsonString = BuildExpectedTypeJsonString();
            var content = new StringContent(expectedJsonString, Encoding.UTF8, "application/json");

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/content/v3/types?id={source}") { Content = content };

            mockRestClient.Setup(c => c.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
            mockRestClient.Setup(c => c.HandleResponse(response));

            // Act
            await repository.SaveTypesAsync();

            // Assert
            mockRestClient.Verify(c => c.SendAsync(It.Is<HttpRequestMessage>(x => Compare(request, x))), Times.Once);
            mockRestClient.Verify(c => c.HandleResponse(response), Times.Once);
            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public async Task SaveTypesAsync_WithLink_ShouldGenerateTypesAndLink()
        {
            // Arrange
            repository.ConfigureContentType<Location>()
                .Field(x => x.Longitude, IndexingType.Queryable)
                .Field(x => x.Latitude, IndexingType.Queryable)
                .Field(x => x.Name, IndexingType.Searchable);

            repository.ConfigureContentType<Event>()
                .Field(x => x.LocationName, IndexingType.Queryable)
                .Field(x => x.Time, IndexingType.Queryable)
                .Field(x => x.Name, IndexingType.Searchable)
                .Field(x => x.AdditionalInfo, IndexingType.PropertyType);

            repository.ConfigureContentType<ExtraInfo>()
                .Field(x => x.Example1, IndexingType.OnlyStored)
                .Field(x => x.Example2, IndexingType.Queryable);

            repository.ConfigureLink<Location, Event>(
                "NameToLocationName",
                x => x.Name,
                x => x.LocationName
            );

            var expectedJsonString = @"{
  ""useTypedFieldNames"": true,
  ""languages"": [],
  ""links"": {
    ""NameToLocationName"": {
      ""from"": ""Name$$String___searchable"",
      ""to"": ""LocationName$$String""
    }
  },
  ""contentTypes"": {
    ""Location"": {
      ""contentType"": [],
      ""properties"": {
        ""Longitude"": {
          ""type"": ""Float"",
          ""searchable"": false,
          ""skip"": false
        },
        ""Latitude"": {
          ""type"": ""Float"",
          ""searchable"": false,
          ""skip"": false
        },
        ""Name"": {
          ""type"": ""String"",
          ""searchable"": true,
          ""skip"": false
        }
      }
    },
    ""Event"": {
      ""contentType"": [],
      ""properties"": {
        ""LocationName"": {
          ""type"": ""String"",
          ""searchable"": false,
          ""skip"": false
        },
        ""Time"": {
          ""type"": ""DateTime"",
          ""searchable"": false,
          ""skip"": false
        },
        ""Name"": {
          ""type"": ""String"",
          ""searchable"": true,
          ""skip"": false
        },
        ""AdditionalInfo"": {
          ""type"": ""ExtraInfo""
        }
      }
    },
    ""ExtraInfo"": {
      ""contentType"": [],
      ""properties"": {
        ""Example1"": {
          ""type"": ""String"",
          ""searchable"": false,
          ""skip"": true
        },
        ""Example2"": {
          ""type"": ""Int"",
          ""searchable"": false,
          ""skip"": false
        }
      }
    }
  },
  ""propertyTypes"": {}
}";

            var jsonString = BuildExpectedTypeJsonString();
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/content/v3/types?id={source}") { Content = content };

            mockRestClient.Setup(c => c.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
            mockRestClient.Setup(c => c.HandleResponse(response));

            // Act
            await repository.SaveTypesAsync();

            // Assert
            Assert.AreEqual(expectedJsonString, jsonString);

            mockRestClient.Verify(c => c.SendAsync(It.Is<HttpRequestMessage>(x => Compare(request, x))), Times.Once);
            mockRestClient.Verify(c => c.HandleResponse(response), Times.Once);
            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public async Task SaveContentAsync_SerializesData_AndCallsGraphClient()
        {
            // Arrange
            repository.ConfigureContentType<ExampleClassObject>()
               .Field(x => x.FirstName, IndexingType.Searchable)
               .Field(x => x.LastName, IndexingType.Searchable)
               .Field(x => x.Age, IndexingType.Queryable)
               .Field(x => x.SubType, IndexingType.PropertyType);

            repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
                .Field(x => x.One, IndexingType.Searchable)
                .Field(x => x.Two, IndexingType.Queryable);

            var exampleData = new ExampleClassObject
            {
                FirstName = "First",
                LastName = "Last",
                Age = 99,
                SubType = new ExampleClassObject.SubType1
                {
                    One = "one",
                    Two = 13
                }
            };

            var expectedJsonString = BuildExpextedContentJsonString(x => x.ToString(), exampleData);

            var content = new StringContent(expectedJsonString, Encoding.UTF8, "application/json");

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/content/v2/data?id={source}") { Content = content };

            mockRestClient.Setup(c => c.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
            mockRestClient.Setup(c => c.HandleResponse(response));

            // Act
            await repository.SaveContentAsync(generateId: (x) => x.ToString(), exampleData);

            // Assert
            mockRestClient.Verify(c => c.SendAsync(It.Is<HttpRequestMessage>(x => Compare(request, x))), Times.Once);
            mockRestClient.Verify(c => c.HandleResponse(response), Times.Once);
            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public async Task SaveContentAsync_WithMultipleTypes_ShouldGenerateJsonForContentOfDifferentTypes()
        {
            // Arrange
            repository.ConfigureContentType<Location>()
                .Field(x => x.Longitude, IndexingType.Queryable)
                .Field(x => x.Latitude, IndexingType.Queryable)
                .Field(x => x.Name, IndexingType.Searchable);

            repository.ConfigureContentType<Event>()
                .Field(x => x.LocationName, IndexingType.Queryable)
                .Field(x => x.Time, IndexingType.Queryable)
                .Field(x => x.Name, IndexingType.Searchable)
                .Field(x => x.AdditionalInfo, IndexingType.PropertyType);

            repository.ConfigureContentType<ExtraInfo>()
                .Field(x => x.Example1, IndexingType.OnlyStored)
                .Field(x => x.Example2, IndexingType.Queryable);

            var locationStockholm = new Location
            {
                Name = "Stockholm",
                Latitude = 59.334591,
                Longitude = 18.063241,
            };
            var locationLondon = new Location
            {
                Name = "London",
                Latitude = 51.5072,
                Longitude = 0.1275,
            };
            var event1 = new Event
            {
                Name = "Future of Project Management",
                Time = new DateTime(2024, 10, 22),
                LocationName = "Stockholm",
                AdditionalInfo = new ExtraInfo
                {
                    Example1 = "test1",
                    Example2 = 1
                }
            };
            var event2 = new Event
            {
                Name = "Week of Hope: Football Camp for Homeless Children in Hanoi!",
                Time = new DateTime(2024, 10, 27),
                LocationName = "Hanoi",
                AdditionalInfo = new ExtraInfo
                {
                    Example1 = "test2",
                    Example2 = 2
                }
            };
            var event3 = new Event
            {
                Name = "Optimizing Project Management: Strategies for Success",
                Time = new DateTime(2024, 11, 03),
                LocationName = "London",
                AdditionalInfo = new ExtraInfo
                {
                    Example1 = "test3",
                    Example2 = 3
                }
            };

            var expectedJsonString = @"{ ""index"": { ""_id"": ""Location-Stockholm"", ""language_routing"": ""en"" } }
{  ""Status$$String"": ""Published"",  ""__typename"": ""Location"",  ""_rbac"": ""r:Everyone:Read"",  ""ContentType$$String"": [    ""Location""  ],  ""Language"": {    ""Name$$String"": ""en""  },  ""Longitude$$Float"": 18.063241,  ""Latitude$$Float"": 59.334591,  ""Name$$String___searchable"": ""Stockholm""}
{ ""index"": { ""_id"": ""Location-London"", ""language_routing"": ""en"" } }
{  ""Status$$String"": ""Published"",  ""__typename"": ""Location"",  ""_rbac"": ""r:Everyone:Read"",  ""ContentType$$String"": [    ""Location""  ],  ""Language"": {    ""Name$$String"": ""en""  },  ""Longitude$$Float"": 0.1275,  ""Latitude$$Float"": 51.5072,  ""Name$$String___searchable"": ""London""}
{ ""index"": { ""_id"": ""Event-Future of Project Management"", ""language_routing"": ""en"" } }
{  ""Status$$String"": ""Published"",  ""__typename"": ""Event"",  ""_rbac"": ""r:Everyone:Read"",  ""ContentType$$String"": [    ""Event""  ],  ""Language"": {    ""Name$$String"": ""en""  },  ""LocationName$$String"": ""Stockholm"",  ""Time$$DateTime"": ""2024-10-21T22:00:00Z"",  ""Name$$String___searchable"": ""Future of Project Management"",  ""AdditionalInfo"": {    ""Example1$$String___skip"": ""test1"",    ""Example2$$Int"": 1  }}
{ ""index"": { ""_id"": ""Event-Week of Hope: Football Camp for Homeless Children in Hanoi!"", ""language_routing"": ""en"" } }
{  ""Status$$String"": ""Published"",  ""__typename"": ""Event"",  ""_rbac"": ""r:Everyone:Read"",  ""ContentType$$String"": [    ""Event""  ],  ""Language"": {    ""Name$$String"": ""en""  },  ""LocationName$$String"": ""Hanoi"",  ""Time$$DateTime"": ""2024-10-26T22:00:00Z"",  ""Name$$String___searchable"": ""Week of Hope: Football Camp for Homeless Children in Hanoi!"",  ""AdditionalInfo"": {    ""Example1$$String___skip"": ""test2"",    ""Example2$$Int"": 2  }}
{ ""index"": { ""_id"": ""Event-Optimizing Project Management: Strategies for Success"", ""language_routing"": ""en"" } }
{  ""Status$$String"": ""Published"",  ""__typename"": ""Event"",  ""_rbac"": ""r:Everyone:Read"",  ""ContentType$$String"": [    ""Event""  ],  ""Language"": {    ""Name$$String"": ""en""  },  ""LocationName$$String"": ""London"",  ""Time$$DateTime"": ""2024-11-02T23:00:00Z"",  ""Name$$String___searchable"": ""Optimizing Project Management: Strategies for Success"",  ""AdditionalInfo"": {    ""Example1$$String___skip"": ""test3"",    ""Example2$$Int"": 3  }}
";

            Func<object, string> generateId = (x) =>
            {
                if (x is Location location)
                {
                    return $"Location-{location.Name}";
                }
                if (x is Event ev)
                {
                    return $"Event-{ev.Name}";
                }
                throw new NotImplementedException();

            };

            var jsonString = BuildExpextedContentJsonString<object>(generateId, locationStockholm, locationLondon, event1, event2, event3);

            var content = new StringContent(expectedJsonString, Encoding.UTF8, "application/json");

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/content/v2/data?id={source}") { Content = content };

            mockRestClient.Setup(c => c.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
            mockRestClient.Setup(c => c.HandleResponse(response));

            // Act
            await repository.SaveContentAsync<object>(generateId, locationStockholm, locationLondon, event1, event2, event3);

            // Assert
            Assert.AreEqual(expectedJsonString, jsonString);

            mockRestClient.Verify(c => c.SendAsync(It.Is<HttpRequestMessage>(x => Compare(request, x))), Times.Once);
            mockRestClient.Verify(c => c.HandleResponse(response), Times.Once);
            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public async Task CreateContent_ShouldContainTwoNewLines()
        {
            // Arrange
            repository.ConfigureContentType<ExampleClassObject>()
               .Field(x => x.FirstName, IndexingType.Searchable)
               .Field(x => x.LastName, IndexingType.Searchable)
               .Field(x => x.Age, IndexingType.Queryable)
               .Field(x => x.SubType, IndexingType.PropertyType);

            repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
                .Field(x => x.One, IndexingType.Searchable)
                .Field(x => x.Two, IndexingType.Queryable);

            var exampleData = new ExampleClassObject
            {
                FirstName = "First",
                LastName = "Last",
                Age = 99,
                SubType = new ExampleClassObject.SubType1
                {
                    One = "type one",
                    Two = 13
                }
            };

            // Act
            var createdContent = repository.CreateContent(generateId: (x) => x.ToString(), exampleData);
            var result = createdContent.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(2, Regex.Matches(result, "\r?\n").Count, "Expected exactly 2 windows- or unix newlines.");
        }

        [TestMethod]
        public async Task CreateContent_ShouldProduceMinifiedContent()
        {
            // Arrange
            repository.ConfigureContentType<ExampleClassObject>()
               .Field(x => x.FirstName, IndexingType.Searchable)
               .Field(x => x.LastName, IndexingType.Searchable)
               .Field(x => x.Age, IndexingType.Queryable)
               .Field(x => x.SubType, IndexingType.PropertyType);

            repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
                .Field(x => x.One, IndexingType.Searchable)
                .Field(x => x.Two, IndexingType.Queryable);

            var exampleData = new ExampleClassObject
            {
                FirstName = "First",
                LastName = "Last",
                Age = 99,
                SubType = new ExampleClassObject.SubType1
                {
                    One = "type one",
                    Two = 13
                }
            };

            // Act
            var createdContent = repository.CreateContent(generateId: (x) => x.ToString(), exampleData);
            var result = createdContent.ReadAsStringAsync().Result;

            // Assert
            var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(3, lines.Length, "Expected exactly 3 lines (1 index, 1 content and 1 empty).");
            Assert.AreEqual("""{"index":{"_id":"Optimizely.Graph.Source.Sdk.Tests.ExampleObjects.ExampleClassObject","language_routing":"en"}}""", lines[0], "Expected index line to be minified (to not contain spaces).");
            Assert.AreEqual("""{"Status$$String":"Published","__typename":"ExampleClassObject","_rbac":"r:Everyone:Read","ContentType$$String":["ExampleClassObject"],"Language":{"Name$$String":"en"},"FirstName$$String___searchable":"First","LastName$$String___searchable":"Last","Age$$Int":99,"SubType":{"One$$String___searchable":"type one","Two$$Int":13}}""", lines[1], "Expected content line to be minified (to not contain spaces).");
            Assert.AreEqual("", lines[2], "Expected empty line to be empty.");
        }
        
        [TestMethod]
        public async Task DeleteContentAsync_ThrowsNotImplementedException()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/content/v2/data?id={source}");

            mockRestClient.Setup(c => c.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
            mockRestClient.Setup(c => c.HandleResponse(response));

            // Act
            await repository.DeleteContentAsync();

            // Assert
            mockRestClient.Verify(c => c.SendAsync(It.Is<HttpRequestMessage>(x => Compare(request, x))), Times.Once);
            mockRestClient.Verify(c => c.HandleResponse(response), Times.Once);
            mockRestClient.VerifyAll();
        }


        #region Private
        private string BuildExpectedTypeJsonString()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new SourceSdkContentTypeConverter()
                }
            };

            return JsonSerializer.Serialize(SourceConfigurationModel.GetTypeFieldConfiguration(), serializeOptions);
        }

        private string BuildExpextedContentJsonString<T>(Func<T, string> generateId, params T[] items)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new SourceSdkContentConverter()
                }
            };

            var itemJson = string.Empty;

            foreach (var data in items)
            {
                itemJson += $"{{ \"index\": {{ \"_id\": \"{generateId(data)}\", \"language_routing\": \"en\" }} }}";
                itemJson += Environment.NewLine;
                itemJson += JsonSerializer.Serialize(data, serializeOptions).Replace("\r\n", "");
                itemJson += Environment.NewLine;
            }
            return itemJson;
        }

        private bool Compare(HttpRequestMessage expected, HttpRequestMessage actual)
        {
            var isMethodEqual = expected.Method.Method == actual.Method.Method;
            var isPathEqual = expected?.RequestUri?.ToString() == actual?.RequestUri?.ToString();

            return isMethodEqual && isPathEqual;
        }
        #endregion
    }
}
