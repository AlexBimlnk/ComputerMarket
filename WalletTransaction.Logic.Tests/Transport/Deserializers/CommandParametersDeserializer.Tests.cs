using General.Logic.Commands;

using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport.Deserializers;

namespace WalletTransaction.Logic.Tests.Transport.Deserializers;
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
                ""type"": ""create_transaction_request"",
                ""id"": ""1"",
                ""request"":
                {
                    ""id"": 1,
                    ""transactions"":
                    [
                        {
                            ""from"": ""01234012340123401234"",
                            ""to"": ""01234012340123401235"",
                            ""transfer_balance"": 123.76,
                            ""held_balance"": 23.76
                        }
                    ]
                }
            }",
            new CreateTransactionRequestCommandParameters(
                new("1"),
                new(new(1),
                    new List<Transaction>()
                    {
                        new Transaction(
                            new("01234012340123401234"),
                            new("01234012340123401235"),
                            123.76m,
                            23.76m)
                    }))
        },
        {
            /*lang=json,strict*/ @"
            {
                ""type"": ""cancel_transaction_request"",
                ""id"": ""1"",
                ""request_id"": 1
            }",
            new CancelTransactionRequestCommandParameters(
                new("1"),
                new(1))
        },
        {
            /*lang=json,strict*/ @"
            {
                ""type"": ""finish_transaction_request"",
                ""id"": ""1"",
                ""request_id"": 1
            }",
            new FinishTransactionRequestCommandParameters(
                new("1"),
                new(1))
        },
    };
}
