using General.Logic.Executables;

using Market.Logic.Commands.WT;

namespace Market.Logic.Tests.Commands;

public class RefundTransactionRequestCommandTests
{
    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        RefundTransactionRequestCommand command = null!;

        var id = new ExecutableID("some id");

        var requestID = new ID(2);

        // Act
        var exception = Record.Exception(() => command =
            new RefundTransactionRequestCommand(
                id,
                requestID));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
        command.TransactionRequestID.Should().Be(requestID);
    }

    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Arrange
        var requestID = new ID(2);

        // Act
        var exception = Record.Exception(() =>
            _ = new RefundTransactionRequestCommand(
                id: null!,
                requestID));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
