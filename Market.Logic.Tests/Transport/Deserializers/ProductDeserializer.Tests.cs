using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Logic.Transport.Deserializers;
using Market.Logic.Transport.Models;

using Newtonsoft.Json;

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
    public void CanDeserialize(string rawSource, ICollection<Product> expectedResult)
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



    public readonly static TheoryData<string, IReadOnlyCollection<Product>> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/@"[
                {""external_id"":1,""internal_id"":1,""provider_id"":1,""price"":100.0,""quantity"":5},
                {""external_id"":1,""internal_id"":4,""provider_id"":2,""price"":55.0,""quantity"":3}
            ]",
            new []
            {
                new Product() {ExternalID = 1, InternalID = 1, ProviderID = 1, Price = 100m, Quantity = 5 },
                new Product() {ExternalID = 1, InternalID = 4, ProviderID = 2, Price = 55m, Quantity = 3 }
            }
        },
        {
            /*lang=json,strict*/@"[]",
            Array.Empty<Product>()
        }
    };
}