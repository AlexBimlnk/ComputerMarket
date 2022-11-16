using General.Logic.Commands;

using WalletTransaction.Logic.Commands;

namespace WalletTransaction.Logic.Tests.Commands;
public class CancelTransactionRequestCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        CancelTransactionRequestCommandParameters parameters = null!;
        var commandId = new CommandID("some id");
        var requestId = new InternalID(1);

        // Act
        var exception = Record.Exception(() => parameters =
            new CancelTransactionRequestCommandParameters(commandId, requestId));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(commandId);
        parameters.TransactionRequestId.Should().Be(requestId);
    }

    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommandParameters)} can't create without command id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCommandId()
    {
        // Arrange
        var requestId = new InternalID(1);

        // Act
        var exception = Record.Exception(() => _ = 
            new CancelTransactionRequestCommandParameters(id: null!, requestId));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
