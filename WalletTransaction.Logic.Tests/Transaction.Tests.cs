namespace WalletTransaction.Logic.Tests;

public class TransactionTests
{
    [Fact(DisplayName = $"The {nameof(Transaction)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Transaction transaction = null!;
        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;
        var heldBalance = 5.1m;

        // Act
        var exception = Record.Exception(() => transaction =
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance,
                heldBalance));

        // Assert
        exception.Should().BeNull();
        transaction.From.Should().BeEquivalentTo(fromAccount);
        transaction.To.Should().BeEquivalentTo(toAccount);
        transaction.TransferBalance.Should().Be(transferredBalance);
        transaction.HeldBalance.Should().Be(heldBalance);
        transaction.IsCompleted.Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(Transaction)} can be created without held balance.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutHeldBalance()
    {
        // Arrange
        Transaction transaction = null!;
        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;

        // Act
        var exception = Record.Exception(() => transaction =
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance));

        // Assert
        exception.Should().BeNull();
        transaction.From.Should().BeEquivalentTo(fromAccount);
        transaction.To.Should().BeEquivalentTo(toAccount);
        transaction.TransferBalance.Should().Be(transferredBalance);
        transaction.HeldBalance.Should().Be(0m);
        transaction.IsCompleted.Should().BeFalse();
    }

    [Theory(DisplayName = $"The {nameof(Transaction)} can't be created without accounts.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(BadAccountsData))]
    public void CanNotBeCreatedWithoutAccounts(BankAccount fromAccount, BankAccount toAccount)
    {
        // Arrange
        var transferredBalance = 121.4m;

        // Act
        var exception = Record.Exception(() => _ =
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(Transaction)} can't be created with bad balance.")]
    [Trait("Category", "Unit")]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void CanNotBeCreatedWithBadBalance(decimal transferredBalance, decimal heldBalance)
    {
        // Arrange
        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");

        // Act
        var exception = Record.Exception(() => _ =
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance,
                heldBalance));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact(DisplayName = $"The {nameof(Transaction)} can be complete.")]
    [Trait("Category", "Unit")]
    public void CanComplete()
    {
        // Arrange
        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;

        var transaction = new Transaction(
            fromAccount,
            toAccount,
            transferredBalance);

        // Act
        var exception = Record.Exception(() => transaction.Complete());

        // Assert
        exception.Should().BeNull();
        transaction.From.Should().BeEquivalentTo(fromAccount);
        transaction.To.Should().BeEquivalentTo(toAccount);
        transaction.TransferBalance.Should().Be(transferredBalance);
        transaction.HeldBalance.Should().Be(0m);
        transaction.IsCompleted.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(Transaction)} can't be complete when already completed.")]
    [Trait("Category", "Unit")]
    public void CanNotCompleteWhenAlreadyCompleted()
    {
        // Arrange
        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;

        var transaction = new Transaction(
            fromAccount,
            toAccount,
            transferredBalance);

        // Act
        transaction.Complete();
        var exception = Record.Exception(() => transaction.Complete());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    public readonly static TheoryData<BankAccount, BankAccount> BadAccountsData = new()
    {
        {
            null!,
            new BankAccount("01234012340123401234")
        },
        {
            new BankAccount("01234012340123401234"),
            null!
        },
        {
            null!,
            null!
        }
    };
}