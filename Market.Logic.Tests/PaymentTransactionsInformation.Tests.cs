using Market.Logic.Models;

namespace Market.Logic.Tests;

public class PaymentTransactionsInformationTests
{
    [Fact(DisplayName = $"The {nameof(PaymentTransactionsInformation)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        PaymentTransactionsInformation information = null!;
        var inn = "0123456789";
        var bankAccount = "01234012340123401234";

        // Act
        var exception = Record.Exception(() => information = new PaymentTransactionsInformation(
            inn,
            bankAccount));

        // Assert
        exception.Should().BeNull();
        information.INN.Should().Be(inn);
        information.BankAccount.Should().Be(bankAccount);
    }

    [Theory(DisplayName = $"The {nameof(PaymentTransactionsInformation)} cannot be created when given bank account has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    [InlineData("abcdeabcdeabcdeabcde")]
    [InlineData("s")]
    [InlineData("012304")]
    [InlineData("012340123401234012341")]
    public void CanNotCreateWhenBankAccountIncorrect(string bankAccount)
    {
        // Act
        var exception = Record.Exception(() => _ = new PaymentTransactionsInformation(
            bankAccount: bankAccount,
            inn: "1234567890"));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(PaymentTransactionsInformation)} cannot be created when given inn has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    [InlineData("123456789")]
    [InlineData("12345678900")]
    [InlineData("abc")]
    [InlineData("abcabcabcc")]
    public void CanNotCreateWhenInnIncorrect(string inn)
    {
        // Act
        var exception = Record.Exception(() => _ = new PaymentTransactionsInformation(
            bankAccount: "01234012340123401234",
            inn: inn));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}