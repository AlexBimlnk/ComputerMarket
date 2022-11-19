using General.Logic.Commands;

using Market.Logic.Commands;
using Market.Logic.Transport.Deserializers;

namespace Market.Logic.Tests.Transport.Deserializers;

public class CommandResultDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(CommandResultDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new CommandResultDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(CommandResultDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, CommandResult expectedResult)
    {
        // Arrange
        var deserializer = new CommandResultDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = $"The {nameof(CommandResultDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new CommandResultDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, CommandResult> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"":
                    {
                        ""value"" : ""id1""
                    },
                    ""isSuccess"":true,
                    ""errorMessage"":null
                }
            ",
            new CommandResult(new CommandID("id1"), errorMessge: null!)

        },
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"":
                    {
                        ""value"" : ""id2""
                    },
                    ""isSuccess"":false,
                    ""errorMessage"" : ""Some error was""
                }
            ",
            new CommandResult(new CommandID("id2"), errorMessge: "Some error was")
        }
    };
}