namespace WalletTransaction.Logic.Tests;

public class TransactionRequestTests
{
    [Fact(DisplayName = $"The {nameof(TransactionRequest)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        TransactionRequest transactionRequest = null!;
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

        // Act
        var exception = Record.Exception(() => transactionRequest =
            new TransactionRequest(id, transactions));

        // Assert
        exception.Should().BeNull();
        transactionRequest.Id.Should().Be(id);
        transactionRequest.Key.Should().Be(id);
        transactionRequest.Transactions.Should().BeEquivalentTo(transactions, opt =>
            opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(TransactionRequest)} can't be created without transactions.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutTransactions()
    {
        // Arrange
        var id = new InternalID(1);

        // Act
        var exception = Record.Exception(() => _ =
            new TransactionRequest(id, transactions: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionRequest)} can't be created with empty transactions collection.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithEmptyTransactionsCollection()
    {
        // Arrange
        var id = new InternalID(1);

        // Act
        var exception = Record.Exception(() => _ =
            new TransactionRequest(id, Array.Empty<Transaction>()));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}