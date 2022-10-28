using Import.Logic.Transport.Deserializers;
using Import.Logic.Transport.Models;

namespace Import.Logic.Tests.Transport.Deserializers;
public class IvanovProductsDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(IvanovProductsDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new IvanovProductsDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(IvanovProductsDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, ExternalProduct[] expectedResult)
    {
        // Arrange
        var deserializer = new IvanovProductsDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = $"The {nameof(IvanovProductsDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new IvanovProductsDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, ExternalProduct[]> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/ @"
            [
                {
                    ""id"": 1,
                    ""name"": ""some name"",
                    ""price"": 100,
                    ""quantity"": 1,
                    ""description"": [ ""some 1"", ""some 2"" ]
                },
                {
                    ""id"": 2,
                    ""name"": ""some name 2"",
                    ""price"": 200,
                    ""quantity"": 5,
                    ""description"": null
                }
            ]",
            new []
            {
                new ExternalProduct
                {
                    Id = 1,
                    Name = "some name",
                    Price = 100m,
                    Quantity = 1,
                    Description = new List<string>{ "some 1", "some 2" }
                },
                new ExternalProduct
                {
                    Id = 2,
                    Name = "some name 2",
                    Price = 200m,
                    Quantity = 5,
                    Description = null!
                },
            }
        },
        {
            /*lang=json,strict*/ @"[]",
            Array.Empty<ExternalProduct>()
        }
    };
}
