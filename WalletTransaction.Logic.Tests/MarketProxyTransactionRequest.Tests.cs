using Moq;

namespace WalletTransaction.Logic.Tests;

public class MarketProxyTransactionRequestTests
{
    [Fact(DisplayName = $"The {nameof(MarketProxyTransactionRequest)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        MarketProxyTransactionRequest proxy = null!;

        var request = Mock.Of<ITransactionsRequest>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => proxy =
            new MarketProxyTransactionRequest(request));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(MarketProxyTransactionRequest)} can't be created without request.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutRequest()
    {
        // Arrange
        MarketProxyTransactionRequest proxy = null!;

        var request = Mock.Of<ITransactionsRequest>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => proxy =
            new MarketProxyTransactionRequest(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(MarketProxyTransactionRequest)} get same id.")]
    [Trait("Category", "Unit")]
    public void ProxyGetSameId()
    {
        // Arrange
        var id = new InternalID(111);

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.Id)
            .Returns(id);

        var proxy = new MarketProxyTransactionRequest(request.Object);

        // Act
        var actual = proxy.Id;

        // Assert
        actual.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(MarketProxyTransactionRequest)} get same id.")]
    [Trait("Category", "Unit")]
    public void ProxyDirectTransactionsToMarketAccount()
    {
        // Arrange
        var fromAccount = new BankAccount("01234012340123401111");
        var toAccount = new BankAccount("01234012340123401999");
        var transferredBalance = 121.4m;
        var heldBalance = 21.4m;

        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                new BankAccount("01234012340123401999"),
                transferredBalance,
                heldBalance),
            new Transaction(
                fromAccount,
                new BankAccount("11234012340123401999"),
                transferredBalance,
                heldBalance),
            new Transaction(
                fromAccount,
                new BankAccount("31234012340123401999"),
                transferredBalance,
                heldBalance)
        };

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.Transactions)
            .Returns(transactions);

        var proxy = new MarketProxyTransactionRequest(request.Object);

        var expectedTransferredBalance = transactions.Select(x => x.TransferBalance)
            .Sum();

        // Act
        var actual = proxy.Transactions.Single();

        // Assert
        actual.From.Should().Be(fromAccount);
        actual.TransferBalance.Should().Be(expectedTransferredBalance);
        actual.HeldBalance.Should().Be(0);
        transactions.Select(x => x.To).Should()
            .AllBeEquivalentTo(MarketProxyTransactionRequest.MarketAccount);
    }

    [Fact(DisplayName = $"The {nameof(MarketProxyTransactionRequest)} capture finished status as held status.")]
    [Trait("Category", "Unit")]
    public void CaptureFinishedStatusAsHeld()
    {
        // Arrange
        var fromAccount = new BankAccount("01234012340123401111");
        var toAccount = new BankAccount("01234012340123401999");
        var transferredBalance = 121.4m;
        var heldBalance = 21.4m;

        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                new BankAccount("01234012340123401999"),
                transferredBalance,
                heldBalance)
        };

        var request = new Mock<ITransactionsRequest>();

        var proxy = new MarketProxyTransactionRequest(request.Object);

        // Act
        proxy.CurrentState = TransactionRequestState.Finished;

        // Assert
        request.VerifySet(x => x.CurrentState = TransactionRequestState.Held, Times.Once());
    }
}