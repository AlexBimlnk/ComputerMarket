using General.Logic.Executables;
using General.Logic.Queries;

using Import.Logic.Models;
using Import.Logic.Queries;
using Import.Logic.Transport.Serializers;

namespace Import.Logic.Tests.Transport.Serializers;
public class QueryResultSerializerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new QueryResultSerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The instance can serialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(SerializeData))]
    public void CanSerialize(IQueryResult products, string expectedResult)
    {
        // Arrange
        var serializer = new QueryResultSerializer();

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
        var serializer = new QueryResultSerializer();

        // Act
        var exception = Record.Exception(() =>
            _ = serializer.Serialize(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    public readonly static TheoryData<IQueryResult, string> SerializeData = new()
    {
        {
            QueryResult<FakeResultObject>.Success(new ExecutableID("123"), new FakeResultObject()
            {
                Value = 1
            }),
            /*lang=json,strict*/@"{""id"":""123"",""error_message"":null,""result"":{""Value"":1}}"
        },
        {
            QueryResult<IReadOnlyCollection<Product>>.Fail(new ExecutableID("123"), "some error name"),
            /*lang=json,strict*/@"{""id"":""123"",""error_message"":""some error name"",""result"":null}"
        }
    };

    private sealed class FakeResultObject
    {
        public int Value { get; set; }
    }
}
