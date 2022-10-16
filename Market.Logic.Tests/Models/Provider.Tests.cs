using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

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
        var information = new PaymentTransactionsInformation("1234567890", "01234012340123401234");

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

    [Theory(DisplayName = $"The {nameof(Provider)} cannot be created when margin less 1.")]
    [Trait("Category", "Unit")]
    [InlineData(0.9)]
    [InlineData(-1)]
    [InlineData(0.00001)]
    [InlineData(0)]
    public void CanNotCreateWhenProviderMarginLessOne(decimal margin)
    {
        // Act
        var exception = Record.Exception(() => _ = new Provider(
            name: "name",
            margin,
            new PaymentTransactionsInformation("1234567890", "01234012340123401234")));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
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
            paymentTransactionsInformation: new PaymentTransactionsInformation("1234567890", "01234012340123401234")));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The can't set new {nameof(Provider)} margin less 1.")]
    [Trait("Category", "Unit")]
    [InlineData(0.9)]
    [InlineData(-1)]
    [InlineData(0.00001)]
    [InlineData(0)]
    public void CanNotSetNewProviderMarginLessTheOne(decimal margin)
    {
        // Arrange
        var name = "Company Name";
        var information = new PaymentTransactionsInformation("1234567890", "01234012340123401234");
        var provider = new Provider(name, margin: 1.1m, information);

        // Act
        var exception = Record.Exception(() => provider.Margin = margin);

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}