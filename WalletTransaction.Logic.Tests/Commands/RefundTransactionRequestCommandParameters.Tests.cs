using General.Logic.Executables;

using WalletTransaction.Logic.Commands;

namespace WalletTransaction.Logic.Tests.Commands;
public class RefundTransactionRequestCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        RefundTransactionRequestCommandParameters parameters = null!;
        var commandId = new ExecutableID("some id");
        var requestId = new InternalID(1);

        // Act
        var exception = Record.Exception(() => parameters =
            new RefundTransactionRequestCommandParameters(commandId, requestId));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(commandId);
        parameters.TransactionRequestId.Should().Be(requestId);
    }

    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommandParameters)} can't create without command id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCommandId()
    {
        // Arrange
        var requestId = new InternalID(1);

        // Act
        var exception = Record.Exception(() => _ =
            new RefundTransactionRequestCommandParameters(id: null!, requestId));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
