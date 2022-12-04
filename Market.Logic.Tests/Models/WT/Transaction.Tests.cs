namespace Market.Logic.Models.WT.Tests;

public class TransactionTests
{
    [Fact(DisplayName = $"The {nameof(Transaction)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Transaction transaction = null!;
        var fromAccount = "01234012340123401234";
        var toAccount = "01234012340123401234";
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
    }

    [Fact(DisplayName = $"The {nameof(Transaction)} can be created without held balance.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutHeldBalance()
    {
        // Arrange
        Transaction transaction = null!;
        var fromAccount = "01234012340123401234";
        var toAccount = "01234012340123401234";
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
    }

    [Theory(DisplayName = $"The {nameof(Transaction)} can't be created without accounts.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(BadAccountsData))]
    public void CanNotBeCreatedWithoutAccounts(string fromAccount, string toAccount)
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
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
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
        var fromAccount = "01234012340123401234";
        var toAccount = "01234012340123401234";

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

    public readonly static TheoryData<string, string> BadAccountsData = new()
    {
        {
            null!,
            "01234012340123401234"
        },
        {
            "01234012340123401234",
            null!
        },
        {
            null!,
            null!
        },
        {
            "\r\t \n",
            "01234012340123401234"
        }
    };
}