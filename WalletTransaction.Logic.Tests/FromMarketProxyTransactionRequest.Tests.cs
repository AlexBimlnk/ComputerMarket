using Moq;

namespace WalletTransaction.Logic.Tests;

public class FromMarketProxyTransactionRequestTests
{
    [Fact(DisplayName = $"The {nameof(FromMarketProxyTransactionRequest)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        FromMarketProxyTransactionRequest proxy = null!;

        var request = Mock.Of<ITransactionsRequest>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => proxy =
            new FromMarketProxyTransactionRequest(request));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(FromMarketProxyTransactionRequest)} can't be created without request.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutRequest()
    {
        // Arrange
        FromMarketProxyTransactionRequest proxy = null!;

        var request = Mock.Of<ITransactionsRequest>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => proxy =
            new FromMarketProxyTransactionRequest(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(FromMarketProxyTransactionRequest)} get same id.")]
    [Trait("Category", "Unit")]
    public void ProxyGetSameId()
    {
        // Arrange
        var id = new InternalID(111);

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.Id)
            .Returns(id);

        var proxy = new FromMarketProxyTransactionRequest(request.Object);

        // Act
        var actual = proxy.Id;

        // Assert
        actual.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(FromMarketProxyTransactionRequest)} get same id.")]
    [Trait("Category", "Unit")]
    public void ProxyDirectTransactionsFromMarketAccount()
    {
        // Arrange
        var fromAccount = new BankAccount("01234012340123401111");
        var transferredBalance = 121.4m;
        var heldBalance = 21.4m;

        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                new BankAccount("01234012340123401997"),
                transferredBalance,
                heldBalance),
            new Transaction(
                fromAccount,
                new BankAccount("11234012340123401996"),
                transferredBalance,
                heldBalance),
            new Transaction(
                fromAccount,
                new BankAccount("31234012340123401995"),
                transferredBalance,
                heldBalance)
        };

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.Transactions)
            .Returns(transactions);

        var proxy = new FromMarketProxyTransactionRequest(request.Object);

        // Act
        var actual = proxy.Transactions;

        // Assert
        actual.Select(x => x.From).Should()
            .AllBeEquivalentTo(FromMarketProxyTransactionRequest.MarketAccount);
        actual.Select(x => x.To).Should()
            .BeEquivalentTo(transactions.Select(x => x.To));
        actual.Select(x => x.TransferBalance).Should()
            .BeEquivalentTo(transactions.Select(x => x.TransferBalance));
        actual.Select(x => x.HeldBalance).Should()
            .BeEquivalentTo(transactions.Select(x => x.HeldBalance));
    }
}