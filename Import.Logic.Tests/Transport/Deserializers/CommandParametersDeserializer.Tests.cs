using General.Logic.Commands;

using Import.Logic.Commands;
using Import.Logic.Models;
using Import.Logic.Transport.Deserializers;

namespace Import.Logic.Tests.Transport.Deserializers;
public class CommandParametersDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(CommandParametersDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new CommandParametersDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(CommandParametersDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, CommandParametersBase expectedParameters)
    {
        // Arrange
        var deserializer = new CommandParametersDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedParameters);
        result.GetType().Should().Be(expectedParameters.GetType());
    }

    [Theory(DisplayName = $"The {nameof(CommandParametersDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new CommandParametersDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(CommandParametersDeserializer)} can't deserialize unknown command type.")]
    [Trait("Category", "Unit")]
    public void CanNotDeserializeUnknownCommandType()
    {
        // Arrange
        var deserializer = new CommandParametersDeserializer();
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

    public readonly static TheoryData<string, CommandParametersBase> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/ @"
            {
                ""type"": ""set_link"",
                ""id"": ""some id"",
                ""internal_id"": 1,
                ""external_id"": 1,
                ""provider_id"": 2
            }",
            new SetLinkCommandParameters(
                new("some id"),
                new(1),
                new(1, Provider.HornsAndHooves))
        },
        {
            /*lang=json,strict*/ @"
            {
            ""type"": ""set_link"",
            ""id"": ""some id"",
            ""internal_id"": 2,
            ""external_id"": 2,
            ""provider_id"": 1
            }",
            new SetLinkCommandParameters(
                new("some id"),
                new(2),
                new(2, Provider.Ivanov))
        },
        {
            /*lang=json,strict*/ @"
            {
                ""type"": ""delete_link"",
                ""id"": ""some id"",
                ""external_id"": 4,
                ""provider_id"": 2
            }",
            new DeleteLinkCommandParameters(
                new("some id"),
                new(4, Provider.HornsAndHooves))
        },
    };
}
