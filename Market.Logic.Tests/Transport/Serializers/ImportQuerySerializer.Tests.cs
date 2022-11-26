using Market.Logic.Queries.Import;
using Market.Logic.Transport.Serializers;

namespace Market.Logic.Tests.Transport.Serializers;
public class ImportQuerySerializerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new ImportQuerySerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The instance can serialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(SerializeData))]
    public void CanSerialize(ImportQuery command, string expectedResult)
    {
        // Arrange
        var serializer = new ImportQuerySerializer();

        string actual = null!;

        // Act
        var exception = Record.Exception(() =>
            actual = serializer.Serialize(command));

        // Assert
        exception.Should().BeNull();
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = $"The instance can't serialize null.")]
    [Trait("Category", "Unit")]
    public void CanNotSerializeNull()
    {
        // Arrange
        var serializer = new ImportQuerySerializer();

        // Act
        var exception = Record.Exception(() =>
            _ = serializer.Serialize(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    public readonly static TheoryData<ImportQuery, string> SerializeData = new()
    {
        {
            new GetLinksQuery(new("some id")),
            
            /*lang=json,strict*/@"{""type"":""get_links"",""id"":""some id""}"
        },
    };
}
