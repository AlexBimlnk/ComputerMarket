using System.Xml;

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
        transactionRequest.IsCancelled.Should().BeFalse();
        transactionRequest.CurrentState.Should().Be(TransactionRequestState.WaitHandle);
        transactionRequest.OldState.Should().Be(TransactionRequestState.WaitHandle);
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

    [Fact(DisplayName = $"The {nameof(TransactionRequest)} can be cancelled.")]
    [Trait("Category", "Unit")]
    public void CanBeCancelled()
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
        var exception = Record.Exception(() => request.IsCancelled = true);

        // Assert
        exception.Should().BeNull();
        request.IsCancelled.Should().BeTrue();
    }

    [Theory(DisplayName = $"The {nameof(TransactionRequest)} can change state.")]
    [InlineData(TransactionRequestState.WaitHandle, TransactionRequestState.WaitHandle)]
    [InlineData(TransactionRequestState.Aborted, TransactionRequestState.Aborted)]
    [InlineData(TransactionRequestState.Refunded, TransactionRequestState.Refunded)]
    [InlineData(TransactionRequestState.Held, TransactionRequestState.Held)]
    [Trait("Category", "Unit")]
    public void CanChangeState(
        TransactionRequestState changeState, 
        TransactionRequestState expectedCurrentStatee)
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
        var exception = Record.Exception(() => request.CurrentState = changeState);

        // Assert
        exception.Should().BeNull();
        request.CurrentState.Should().Be(expectedCurrentStatee);
        request.OldState.Should().Be(TransactionRequestState.WaitHandle);
    }

    [Fact(DisplayName = $"The {nameof(TransactionRequest)} can't be cancelled if request already have been cancelled.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCancelledWhenAlreadyCancelled()
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
        request.IsCancelled = true;

        // Act
        var exception = Record.Exception(() => request.IsCancelled = true);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        request.IsCancelled.Should().BeTrue();
    }
}