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
        var id = new ID(1);
        var name = "Company Name";
        var margin = new Margin(1m);
        var information = new PaymentTransactionsInformation("1234567890", "01234012340123401234");

        // Act
        var exception = Record.Exception(() => provider = new Provider(
            id,
            name,
            margin,
            information));

        // Assert
        exception.Should().BeNull();
        provider.Key.Should().Be(id);
        provider.Name.Should().Be(name);
        provider.Margin.Should().Be(margin);
        provider.PaymentTransactionsInformation.Should().Be(information);
    }

    [Fact(DisplayName = $"The {nameof(Provider)} cannot create witout {nameof(PaymentTransactionsInformation)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutPaymentInformation()
    {
        // Act
        var exception = Record.Exception(() => _ = new Provider(
            id: new ID(1),
            name: "Name",
            margin: new Margin(1m),
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
            id: new ID(1),
            name: name,
            margin: new Margin(1m),
            paymentTransactionsInformation: new PaymentTransactionsInformation("1234567890", "01234012340123401234")));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}