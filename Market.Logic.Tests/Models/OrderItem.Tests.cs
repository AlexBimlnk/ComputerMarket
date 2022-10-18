using System.Reflection;

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
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: quantity);

        // Act
        var exception = Record.Exception(() => item = new OrderItem(product, quantity));

        // Assert
        exception.Should().BeNull();
        item.Product.Should().Be(product);
        item.Quantity.Should().Be(quantity);
        item.Selected.Should().BeTrue();
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

    [Theory(DisplayName = $"The {nameof(OrderItem)} cannot be created with quantity less than one or greater than product quantity.")]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(10000000)]
    public void CanNotCreateWhenQuantityLessThanOne(int quantity)
    {
        // Arrange
        var product = new Product(
            item: new Item(
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                "provider_name",
                new Margin(1.1m),
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

    [Fact(DisplayName = $"The {nameof(OrderItem)} can increase product quantity.")]
    [Trait("Category", "Unit")]
    public void CanIncreaseQuantity()
    {
        // Arrange
        var quantity = 1;
        var product = new Product(
            item: new Item(
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        var item = new OrderItem(product, quantity);
        var expectedResult = 3;

        // Act
        item.IncQuantity();
        item.IncQuantity();
        var result = item.Quantity;
        item.IncQuantity();
        item.IncQuantity();
        item.IncQuantity();
        item.IncQuantity();

        // Assert
        result.Should().Be(expectedResult);
        item.Quantity.Should().Be(product.Quantity);
    }

    [Fact(DisplayName = $"The {nameof(OrderItem)} can decrease product quantity.")]
    [Trait("Category", "Unit")]
    public void CanDecreaseQuantity()
    {
        // Arrange
        var quantity = 4;
        var product = new Product(
            item: new Item(
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        var item = new OrderItem(product, quantity);
        var expectedResult = 2;

        // Act
        item.DecQuantity();
        item.DecQuantity();
        var result = item.Quantity;
        item.DecQuantity();
        item.DecQuantity();
        item.DecQuantity();

        // Assert
        result.Should().Be(expectedResult);
        item.Quantity.Should().Be(1);
    }
}
