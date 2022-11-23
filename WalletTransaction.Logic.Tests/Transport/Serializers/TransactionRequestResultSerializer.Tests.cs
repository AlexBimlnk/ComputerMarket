using Moq;

using WalletTransaction.Logic.Transport.Serializers;

namespace WalletTransaction.Logic.Tests.Transport.Serializers;
public class TransactionRequestResultSerializerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new TransactionRequestResultSerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can serialize.")]
    [Trait("Category", "Unit")]
    public void CanSerialize()
    {
        // Arrange
        var serializer = new TransactionRequestResultSerializer();

        var transactionRequest = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        transactionRequest.SetupGet(x => x.Id)
            .Returns(new InternalID(1));
        transactionRequest.SetupGet(x => x.IsCancelled)
            .Returns(true);
        transactionRequest.SetupGet(x => x.CurrentState)
            .Returns(TransactionRequestState.WaitHandle);

        var expected = /*lang=json,strict*/
            @"{""id"":1,""is_cancelled"":true,""last_state"":""wait_handle""}";

        string actual = null!;

        // Act
        var exception = Record.Exception(() =>
            actual = serializer.Serialize(transactionRequest.Object));

        // Assert
        exception.Should().BeNull();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact(DisplayName = $"The instance can't serialize null.")]
    [Trait("Category", "Unit")]
    public void CanNotSerializeNull()
    {
        // Arrange
        var serializer = new TransactionRequestResultSerializer();

        // Act
        var exception = Record.Exception(() =>
            _ = serializer.Serialize(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
