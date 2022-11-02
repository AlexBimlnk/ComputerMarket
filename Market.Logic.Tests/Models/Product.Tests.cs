using System.Net.WebSockets;

using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ProductTests
{
    [Fact(DisplayName = $"The {nameof(Product)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var providerId = new InternalID(2); 
        var itemId = new InternalID(1);
        Product product = null!;
        var provider = new Provider(
            providerId,
            "provider_name",
            new Margin(1.1m),
            new PaymentTransactionsInformation(
                "0123456789",
                "01234012340123401234"));

        var item = new Item(
            itemId,
            new ItemType("some_type"),
            "some_name",
            properties: Array.Empty<ItemProperty>());

        var price = new Price(100m);
        var quantity = 5;

        // Act
        var exception = Record.Exception(() => product = new Product(
            item,
            provider,
            price,
            quantity));

        // Assert
        exception.Should().BeNull();
        product.Key.Should().Be((itemId.Value, providerId.Value));
        product.Item.Should().Be(item);
        product.Provider.Should().Be(provider);
        product.Quantity.Should().Be(quantity);
        product.ProviderCost.Should().Be(100m);
        product.FinalCost.Should().Be(100m * 1.1m);
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created when quantity less 0.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenQuantityLessZero()
    {
        // Arrange
        var provider = new Provider(
            id: new InternalID(1),
            "provider_name",
            new Margin(1.1m),
            new PaymentTransactionsInformation(
                "0123456789",
                "01234012340123401234"));

        var item = new Item(
            id: new InternalID(1),
            new ItemType("some_type"),
            "some_name",
            properties: Array.Empty<ItemProperty>());

        // Act
        var exception = Record.Exception(() => _ = new Product(
            item,
            provider,
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created without item.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutItem()
    {
        // Arrange
        var provider = new Provider(
            id: new InternalID(1),
            "provider_name",
            new Margin(1.1m),
            new PaymentTransactionsInformation(
                "0123456789",
                "01234012340123401234"));

        // Act
        var exception = Record.Exception(() => _ = new Product(
            item: null!,
            provider,
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created without provider.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProvider()
    {
        // Arrange
        var item = new Item(
            id: new InternalID(1),
            new ItemType("some_type"),
            "some_name",
            properties: Array.Empty<ItemProperty>());

        // Act
        var exception = Record.Exception(() => _ = new Product(
            item,
            provider: null!,
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
