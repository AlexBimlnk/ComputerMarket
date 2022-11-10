namespace WalletTransaction.Logic.Tests;

public class BankAccountTests
{
    [Fact(DisplayName = $"The {nameof(BankAccount)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        BankAccount bankAccount = null!;

        // Act
        var exception = Record.Exception(() => 
            bankAccount = new BankAccount("01234012340123401234"));

        // Assert
        exception.Should().BeNull();
        bankAccount.Value.Should().Be("01234012340123401234");
    }

    [Theory(DisplayName = $"The {nameof(BankAccount)} cannot be created when given bank account value has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    [InlineData("abcdeabcdeabcdeabcde")]
    [InlineData("s")]
    [InlineData("012304")]
    [InlineData("012340123401234012341")]
    public void CanNotCreateWhenBankAccountValueIncorrect(string bankAccount)
    {
        // Act
        var exception = Record.Exception(() => _ = new BankAccount(bankAccount));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}