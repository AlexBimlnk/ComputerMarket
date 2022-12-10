using Market.Logic.Models.WT;
using Market.Logic.Transport.Deserializers;

namespace Market.Logic.Tests.Transport.Deserializers;

public class TransactionResultDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(TransactionResultDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new TransactionResultDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(TransactionResultDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, TransactionRequestResult expectedResult)
    {
        // Arrange
        var deserializer = new TransactionResultDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = $"The {nameof(TransactionResultDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new TransactionResultDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, TransactionRequestResult> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"": 1,
                    ""is_cancelled"": true,
                    ""last_state"": ""aborted""
                }
            ",
            new TransactionRequestResult(
                new ID(1),
                isCancelled: true,
                TransactionRequestState.Aborted)
        },
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"": 1,
                    ""is_cancelled"": false,
                    ""last_state"": ""finished""
                }
            ",
            new TransactionRequestResult(
                new ID(1),
                isCancelled: false,
                TransactionRequestState.Finished)
        },
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"": 2,
                    ""is_cancelled"": false,
                    ""last_state"": ""refunded""
                }
            ",
            new TransactionRequestResult(
                new ID(2),
                isCancelled: false,
                TransactionRequestState.Refunded)
        },
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"": 2,
                    ""is_cancelled"": true,
                    ""last_state"": ""held""
                }
            ",
            new TransactionRequestResult(
                new ID(2),
                isCancelled: true,
                TransactionRequestState.Held)
        },
    };
}