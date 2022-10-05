using Market.Logic.Models;

namespace Market.Logic.Tests;

public class ProviderTests
{
    [Fact(DisplayName = $"The {nameof(Provider)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Provider provider = null!;
        var name = "Company Name";
        var margin = 1.1m;
        var information = new PaymentTransactionsInformation("1234567890", "1234");

        // Act
        var exception = Record.Exception(() => provider = new Provider(
            name,
            margin,
            information));

        // Assert
        exception.Should().BeNull();
        provider.Name.Should().Be(name);
        provider.Margin.Should().Be(margin);
        provider.PaymentTransactionsInformation.Should().Be(information);
    }

    [Fact(DisplayName = $"The {nameof(Provider)} cannot be created when margin less 1.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenProviderMarginLessOne()
    {
        // Act
        var exception = Record.Exception(() => _ = new Provider(
            name: "name",
            margin: 0.1m,
            paymentTransactionsInformation: new PaymentTransactionsInformation("1234567890", "1234")));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(Provider)} cannot create witout {nameof(PaymentTransactionsInformation)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutPaymentInformation()
    {
        // Act
        var exception = Record.Exception(() => _ = new Provider(
            name: "Name",
            margin: 1m,
            paymentTransactionsInformation: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(Provider)} cannot be created when given provider name has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    public void CanNotCreateWhenProviderNameIncorrect(string name)
    {
        // Act
        var exception = Record.Exception(() => _ = new Provider(
            name: name,
            margin: 1m,
            paymentTransactionsInformation: new PaymentTransactionsInformation("1234567890", "1234")));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}