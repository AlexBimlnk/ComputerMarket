using Market.Logic.Commands.Import;
using Market.Logic.Commands.WT;
using Market.Logic.Models.WT;
using Market.Logic.Transport.Serializers;

namespace Market.Logic.Tests.Transport.Serializers;
public class WTCommandSerializerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new WTCommandSerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The instance can serialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(SerializeData))]
    public void CanSerialize(WTCommand command, string expectedResult)
    {
        // Arrange
        var serializer = new WTCommandSerializer();

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
        var serializer = new WTCommandSerializer();

        // Act
        var exception = Record.Exception(() =>
            _ = serializer.Serialize(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    public readonly static TheoryData<WTCommand, string> SerializeData = new()
    {
        {
            new CreateTransactionRequestCommand(
                new("some id"),
                requestID: new(1),
                transactions: new[]
                {
                    new Transaction("from", "to", 123.7m, 12),
                    new Transaction("from", "to", 123.1m),
                }),
            "{" +
               "\"request\":" +
               "{" +
                    "\"id\":1," +
                    "\"transactions\":[" +
                        "{\"from\":\"from\",\"to\":\"to\",\"transfer_balance\":123.7,\"held_balance\":12.0}," +
                        "{\"from\":\"from\",\"to\":\"to\",\"transfer_balance\":123.1,\"held_balance\":0.0}" +
                    "]" +
                "}," +
                "\"type\":\"create_transaction_request\"," +
                "\"id\":\"some id\"" +
            "}"
        },
        {
            new FinishTransactionRequestCommand(
                new("some id"),
                requestID: new(1)),
            
            /*lang=json,strict*/@"{""request_id"":1,""type"":""finish_transaction_request"",""id"":""some id""}"
        },
        {
            new RefundTransactionRequestCommand(
                new("some id"),
                requestID: new(1)),
            
            /*lang=json,strict*/@"{""request_id"":1,""type"":""refund_transaction_request"",""id"":""some id""}"
        },
        {
            new CancelTransactionRequestCommand(
                new("some id"),
                requestID: new(1)),
            
            /*lang=json,strict*/@"{""request_id"":1,""type"":""cancel_transaction_request"",""id"":""some id""}"
        }
    };
}
