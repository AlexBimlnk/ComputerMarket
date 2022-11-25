using General.Logic.Commands;
using General.Logic.Executables;

using WalletTransaction.Logic.Commands;

namespace WalletTransaction.Logic.Tests.Commands;
public class FinishTransactionRequestCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        FinishTransactionRequestCommandParameters parameters = null!;
        var commandId = new ExecutableID("some id");
        var requestId = new InternalID(1);

        // Act
        var exception = Record.Exception(() => parameters =
            new FinishTransactionRequestCommandParameters(commandId, requestId));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(commandId);
        parameters.TransactionRequestId.Should().Be(requestId);
    }

    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommandParameters)} can't create without command id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCommandId()
    {
        // Arrange
        var requestId = new InternalID(1);

        // Act
        var exception = Record.Exception(() => _ = 
            new FinishTransactionRequestCommandParameters(id: null!, requestId));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
