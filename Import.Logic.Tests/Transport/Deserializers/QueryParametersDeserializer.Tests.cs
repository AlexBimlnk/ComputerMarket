using General.Logic.Queries;

using Import.Logic.Queries;
using Import.Logic.Transport.Deserializers;

namespace Import.Logic.Tests.Transport.Deserializers;
public class QueryParametersDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(QueryParametersDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new QueryParametersDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(QueryParametersDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, QueryParametersBase expectedParameters)
    {
        // Arrange
        var deserializer = new QueryParametersDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedParameters);
        result.GetType().Should().Be(expectedParameters.GetType());
    }

    [Theory(DisplayName = $"The {nameof(QueryParametersDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new QueryParametersDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(QueryParametersDeserializer)} can't deserialize unknown command type.")]
    [Trait("Category", "Unit")]
    public void CanNotDeserializeUnknownCommandType()
    {
        // Arrange
        var deserializer = new QueryParametersDeserializer();
        var rawSource = /*lang=json,strict*/ @"
        {
            ""type"": ""unknown_command"",
            ""id"": ""some id"",
            ""internal_id"": 1,
            ""external_id"": 1,
            ""provider_id"": 1
        }";

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(rawSource));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, QueryParametersBase> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/ @"
            {
                ""type"": ""get_links"",
                ""id"": ""some id""
            }",
            new GetLinksQueryParameters(new("some id"))
        },
    };
}
