using Import.Logic.Models;
using Import.Logic.Transport.Serializers;

namespace Import.Logic.Tests.Transport.Serializers;
public class ProductsSerializerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new ProductsSerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The instance can serialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(SerializeData))]
    public void CanSerialize(IReadOnlyCollection<Product> products, string expectedResult)
    {
        // Arrange
        var serializer = new ProductsSerializer();

        string actual = null!;

        // Act
        var exception = Record.Exception(() =>
            actual = serializer.Serialize(products));

        // Assert
        exception.Should().BeNull();
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = $"The instance can't serialize null.")]
    [Trait("Category", "Unit")]
    public void CanNotSerializeNull()
    {
        // Arrange
        var serializer = new ProductsSerializer();

        // Act
        var exception = Record.Exception(() =>
            _ = serializer.Serialize(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    public readonly static TheoryData<IReadOnlyCollection<Product>, string> SerializeData = new()
    {
        {
            new []
            {
                new Product(new(1, Provider.Ivanov), new(100), 5).MappedTo(new(1)),
                new Product(new(1, Provider.HornsAndHooves), new(55), 3).MappedTo(new(4)),
            },
            /*lang=json,strict*/@"[{""external_id"":1,""internal_id"":1,""provider_name"":""ivanov"",""price"":100.0,""quantity"":5},{""external_id"":1,""internal_id"":4,""provider_name"":""horns_and_hooves"",""price"":55.0,""quantity"":3}]"
        },
        {
            Array.Empty<Product>(),
            /*lang=json,strict*/@"[]"
        }
    };
}
