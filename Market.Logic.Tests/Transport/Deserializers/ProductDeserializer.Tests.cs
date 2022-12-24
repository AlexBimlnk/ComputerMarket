using Market.Logic.Models;
using Market.Logic.Transport.Deserializers;
using Market.Logic.Transport.Models;

namespace Market.Logic.Tests.Transport.Deserializers;

public class ProductDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(ProductDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new ProductDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(ProductDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, ICollection<UpdateByProduct> expectedResult)
    {
        // Arrange
        var deserializer = new ProductDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = $"The {nameof(ProductDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new ProductDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, IReadOnlyCollection<UpdateByProduct>> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/
            @"[
                {""external_id"":1,""internal_id"":1,""provider_id"":1,""price"":100.0,""quantity"":5},
                {""external_id"":1,""internal_id"":4,""provider_id"":2,""price"":55.0,""quantity"":3}
            ]",
            new []
            {
                new UpdateByProduct(
                    externalID: new(1),
                    internalID: new(1),
                    providerID: new(1),
                    new Price(100.0m),
                    quantity: 5
                ),
                new UpdateByProduct(
                    externalID: new(1),
                    internalID: new(4),
                    providerID: new(2),
                    new Price(55.0m),
                    quantity: 3
                )
            }
        },
        {
            /*lang=json,strict*/@"[]",
            Array.Empty<UpdateByProduct>()
        }
    };
}