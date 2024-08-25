using Moq;
using System.Diagnostics.CodeAnalysis;
using Optimizely.Graph.Source.Sdk.Repositories;
using Optimizely.Graph.Source.Sdk.Models;
using Optimizely.Graph.Source.Sdk.JsonConverters;
using System.Text.Json;

namespace Optimizely.Graph.Source.Sdk.Tests.RepositoryTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GraphSourceRepositoryTests
    {
        //private GraphSourceRepository repository;

        //public GraphSourceRepositoryTests()
        //{
        //    //mockClient = new Mock<IContentGraphClient>();
        //    //repository = new GraphSourceRepository(mockClient.Object);
        //}

        //[TestMethod]
        //public void AddLanguage_SetsExpectedValue_InSourceConfigurationModel()
        //{
        //    // Arrange
        //    var language = "en";

        //    // Act
        //    repository.AddLanguage(language);

        //    // Assert
        //    var actualLanguages = SourceConfigurationModel.GetLanguages();

        //    Assert.AreEqual(1, actualLanguages.Count());
        //    Assert.IsTrue(actualLanguages.Contains(language));
        //}

        //[TestMethod]
        //public void ConfigureContentType_SetsExpectedContentTypes_InSourceConfigurationModel()
        //{
        //    // Arrange & Act
        //    repository.ConfigureContentType<ExampleClassObject>()
        //        .Field(x => x.FirstName, IndexingType.Searchable)
        //        .Field(x => x.LastName, IndexingType.Searchable)
        //        .Field(x => x.Age, IndexingType.Querable)
        //        .Field(x => x.SubType, IndexingType.PropertyType);


        //    // Assert
        //    var contentTypes = SourceConfigurationModel.GetContentTypeFieldConfiguration();

        //    Assert.AreEqual(contentTypes.First().ConfigurationType, ConfigurationType.ContentType);
        //    Assert.AreEqual(contentTypes.First().TypeName, nameof(ExampleClassObject));
        //    Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "FirstName" && x.IndexingType == IndexingType.Searchable));
        //    Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "LastName" && x.IndexingType == IndexingType.Searchable));
        //    Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "Age" && x.IndexingType == IndexingType.Querable));
        //    Assert.IsTrue(contentTypes.First().Fields.Any(x => x.Name == "SubType" && x.IndexingType == IndexingType.PropertyType));
        //    Assert.AreEqual(4, contentTypes.First().Fields.Count);
        //}

        //[TestMethod]
        //public void ConfigurePropertyType_SetsExpectedPropertyTypes_InSourceConfigurationModel()
        //{
        //    // Arrange & Act
        //    repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
        //        .Field(x => x.One, IndexingType.Searchable)
        //        .Field(x => x.Two, IndexingType.Querable);

        //    // Assert
        //    var propertyTypes = SourceConfigurationModel.GetPropertyTypeFieldConfiguration();

        //    Assert.AreEqual(propertyTypes.First().ConfigurationType, ConfigurationType.PropertyType);
        //    Assert.AreEqual(propertyTypes.First().TypeName, "SubType1");
        //    Assert.IsTrue(propertyTypes.First().Fields.Any(x => x.Name == "One" && x.IndexingType == IndexingType.Searchable));
        //    Assert.IsTrue(propertyTypes.First().Fields.Any(x => x.Name == "Two" && x.IndexingType == IndexingType.Querable));
        //    Assert.AreEqual(2, propertyTypes.First().Fields.Count);
        //}

        //[TestMethod]
        //public async Task SaveTypesAsync_BuildsExpectedJsonString_AndCallsGraphClient()
        //{
        //    // Arrange
        //    repository.ConfigureContentType<ExampleClassObject>()
        //        .Field(x => x.FirstName, IndexingType.Searchable)
        //        .Field(x => x.LastName, IndexingType.Searchable)
        //        .Field(x => x.Age, IndexingType.Querable);

        //    var expectedJsonString = BuildExpectedTypeJsonString();

        //    mockClient.Setup(c => c.SendTypesAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);

        //    // Act
        //    await repository.SaveTypesAsync();

        //    // Assert
        //    mockClient.Verify(c => c.SendTypesAsync(expectedJsonString), Times.Once);
        //}

        //[TestMethod]
        //public async Task SaveContentAsync_SerializesData_AndCallsGraphClient()
        //{
        //    // Arrange
        //    repository.ConfigureContentType<ExampleClassObject>()
        //       .Field(x => x.FirstName, IndexingType.Searchable)
        //       .Field(x => x.LastName, IndexingType.Searchable)
        //       .Field(x => x.Age, IndexingType.Querable)
        //       .Field(x => x.SubType, IndexingType.PropertyType);

        //    repository.ConfigurePropertyType<ExampleClassObject.SubType1>()
        //        .Field(x => x.One, IndexingType.Searchable)
        //        .Field(x => x.Two, IndexingType.Querable);

        //    var exampleData = new ExampleClassObject
        //    {
        //        FirstName = "First",
        //        LastName = "Last",
        //        Age = 99,
        //        SubType = new ExampleClassObject.SubType1
        //        {
        //            One = "one",
        //            Two = 13
        //        }
        //    };

        //    var expectedJsonString = BuildExpextedContentJsonString(exampleData);

        //    mockClient.Setup(c => c.SendContentBulkAsync(It.IsAny<string>())).ReturnsAsync("some/string");

        //    // Act
        //    await repository.SaveContentAsync(generateId: (x) => x.ToString(), exampleData);

        //    mockClient.Verify(c => c.SendContentBulkAsync(expectedJsonString), Times.Once);
        //}

        //[TestMethod]
        //public async Task DeleteContentAsync_ThrowsNotImplementedException()
        //{
        //    await Assert.ThrowsExceptionAsync<NotImplementedException>(() => this.repository.DeleteContentAsync("id"));
        //}

        //private string BuildExpectedTypeJsonString()
        //{
        //    var serializeOptions = new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        Converters =
        //        {
        //            new SourceSdkContentTypeConverter()
        //        }
        //    };
            
        //    return JsonSerializer.Serialize(SourceConfigurationModel.GetTypeFieldConfiguration(), serializeOptions);
        //}

        //private string BuildExpextedContentJsonString(ExampleClassObject data)
        //{
        //    var serializeOptions = new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        Converters =
        //        {
        //            new SourceSdkContentConverter()
        //        }
        //    };

        //    var itemJson = string.Empty;

        //    itemJson += $"{{ \"index\": {{ \"_id\": \"Optimizely.Graph.Source.Sdk.Tests.ExampleClassObject\", \"language_routing\": \"en\" }} }}";
        //    itemJson += Environment.NewLine;
        //    itemJson += JsonSerializer.Serialize(data, serializeOptions).Replace("\r\n", "");
        //    itemJson += Environment.NewLine;

        //    return itemJson;
        //}
    }
}
