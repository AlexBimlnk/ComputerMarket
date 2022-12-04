using General.Logic.Executables;

using Market.Logic.Commands.WT;
using Market.Logic.Models.WT;

namespace Market.Logic.Tests.Commands;

public class CreateTransactionRequestCommandTests
{
    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        CreateTransactionRequestCommand command = null!;

        var id = new ExecutableID("some id");

        var requestID = new ID(2);

        var transactions = new[]
        {
            new Transaction(
                "from", 
                "to",
                transferBalance: 121.2m,
                heldBalance: 21.2m)
        };

        // Act
        var exception = Record.Exception(() => command =
            new CreateTransactionRequestCommand(
                id,
                requestID,
                transactions));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
        command.TransactionRequestID.Should().Be(requestID);
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Arrange
        var requestID = new ID(2);

        // Act
        var exception = Record.Exception(() =>
            _ = new CreateTransactionRequestCommand(
                id: null!,
                requestID,
                Array.Empty<Transaction>()));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can't create without transactons.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutTransactions()
    {
        // Arrange
        var id = new ExecutableID("some id");
        var requestID = new ID(2);

        // Act
        var exception = Record.Exception(() =>
            _ = new CreateTransactionRequestCommand(
                id,
                requestID,
                transactions: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
