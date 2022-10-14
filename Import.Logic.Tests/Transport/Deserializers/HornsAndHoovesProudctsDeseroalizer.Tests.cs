using Import.Logic.Transport.Deserializers;
using Import.Logic.Transport.Models;

namespace Import.Logic.Tests.Transport.Deserializers;
public class HornsAndHoovesProductsDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(HornsAndHoovesProductsDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new HornsAndHoovesProductsDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(HornsAndHoovesProductsDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeData))]
    public void CanDeserialize(string rawSource, HornsAndHoovesProduct[] expectedResult)
    {
        // Arrange
        var deserializer = new HornsAndHoovesProductsDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = $"The {nameof(HornsAndHoovesProductsDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new HornsAndHoovesProductsDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, HornsAndHoovesProduct[]> DeserializeData = new()
    {
        {
            /*lang=json,strict*/ @"
            [
                {
                    ""product_id"": 1,
                    ""product_name"": ""some name"",
                    ""product_price"": 100,
                    ""product_quantity"": 1,
                    ""product_description"": ""some description"",
                    ""product_type"": ""some type""
                },
                {
                    ""product_id"": 2,
                    ""product_name"": ""some name 2"",
                    ""product_price"": 123.57,
                    ""product_quantity"": 11,
                    ""product_description"": ""some description 2"",
                    ""product_type"": ""some type 2""
                }
            ]",
            new []
            {
                new HornsAndHoovesProduct
                {
                    Id = 1,
                    Name = "some name",
                    Price = 100m,
                    Quantity = 1,
                    Description = "some description",
                    Type = "some type"
                },
                new HornsAndHoovesProduct
                {
                    Id = 2,
                    Name = "some name 2",
                    Price = 123.57m,
                    Quantity = 11,
                    Description = "some description 2",
                    Type = "some type 2"
                },
            }
        },
        {
            /*lang=json,strict*/ @"[]",
            Array.Empty<HornsAndHoovesProduct>()
        }
    };
}
