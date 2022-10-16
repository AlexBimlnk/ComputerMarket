using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Logic.Models;

namespace Market.Logic.Tests.Models;
public class OrderItemTests
{
    [Fact(DisplayName = $"The {nameof(OrderItem)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        OrderItem item = null!;
        var quantity = 10;
        var product = new Product(
            item: new Item(
                new ItemType("some_type"),
                "some_name",
                properties: Enumerable.Empty<ItemProperty>()),
            provider: new Provider(
                "provider_name",
                1.1m,
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        // Act
        var exception = Record.Exception(() => item = new OrderItem(product, quantity));

        // Assert
        exception.Should().BeNull();
        item.Product.Should().Be(product);
        item.Quantity.Should().Be(quantity);
    }

    [Fact(DisplayName = $"The {nameof(OrderItem)} cannot be created without {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreatWithoutProduct()
    {
        // Act
        var exception = Record.Exception(() => _ = new OrderItem(product: null!, quantity: 1));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(OrderItem)} cannot be created with empty or null or white spaced value.")]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CanNotCreateWhenQuantityLessThanOne(int quantity)
    {
        // Arrange
        var product = new Product(
            item: new Item(
                new ItemType("some_type"),
                "some_name",
                properties: Enumerable.Empty<ItemProperty>()),
            provider: new Provider(
                "provider_name",
                1.1m,
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        // Act
        var exception = Record.Exception(() => _ = new OrderItem(product, quantity));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}
