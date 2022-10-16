using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ProductTests
{
    [Fact(DisplayName = $"The {nameof(Product)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Product product = null!;
        var provider = new Provider(
            "provider_name", 
            1.1m, 
            new PaymentTransactionsInformation(
                "0123456789",
                "01234012340123401234"));

        var item = new Item(
            new ItemType("some_type"),
            "some_name",
            properties: Enumerable.Empty<ItemProperty>());

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
            "provider_name",
            1.1m,
            new PaymentTransactionsInformation(
                "0123456789",
                "01234012340123401234"));

        var item = new Item(
            new ItemType("some_type"),
            "some_name",
            properties: Enumerable.Empty<ItemProperty>());

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
            "provider_name",
            1.1m,
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
            new ItemType("some_type"),
            "some_name",
            properties: Enumerable.Empty<ItemProperty>());

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
