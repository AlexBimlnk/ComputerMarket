using General.Logic.Commands;
using General.Logic.Executables;

using WalletTransaction.Logic.Commands;

namespace WalletTransaction.Logic.Tests.Commands;
public class CreateTransactionRequestCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        CreateTransactionRequestCommandParameters parameters = null!;
        var commandId = new ExecutableID("some id");
        var id = new InternalID(1);

        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;
        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance)
        };

        var request = new TransactionRequest(id, transactions);

        // Act
        var exception = Record.Exception(() => parameters =
            new CreateTransactionRequestCommandParameters(commandId, request));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(commandId);
        parameters.TransactionRequest.Should().Be(request);
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommandParameters)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Arrange
        var id = new InternalID(1);

        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;
        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance)
        };

        var request = new TransactionRequest(id, transactions);

        // Act
        var exception = Record.Exception(() => _ = new CreateTransactionRequestCommandParameters(
            id: null!,
            request));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommandParameters)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutTransactionRequest()
    {
        // Act
        var exception = Record.Exception(() => _ = new CreateTransactionRequestCommandParameters(
            new ExecutableID("some id"),
            transactionRequest: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
