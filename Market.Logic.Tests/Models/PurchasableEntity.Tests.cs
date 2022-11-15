using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class PurchasableEntityTests
{
    [Fact(DisplayName = $"The {nameof(PurchasableEntity)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        PurchasableEntity entity = null!;
        var quantity = 10;
        var product = new Product(
            item: new Item(
                new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: quantity);

        // Act
        var exception = Record.Exception(() => entity = new PurchasableEntity(product, quantity));

        // Assert
        exception.Should().BeNull();
        entity.Product.Should().Be(product);
        entity.Quantity.Should().Be(quantity);
        entity.Selected.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(PurchasableEntity)} cannot be created without {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreatWithoutProduct()
    {
        // Act
        var exception = Record.Exception(() => _ = new PurchasableEntity(product: null!, quantity: 1));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(PurchasableEntity)} cannot be created with quantity less than one or greater than product quantity.")]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(10000000)]
    public void CanNotCreateWhenQuantityLessThanOneOrGreaterThanProductQuantity(int quantity)
    {
        // Arrange
        var product = new Product(
            item: new Item(
                new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        // Act
        var exception = Record.Exception(() => _ = new PurchasableEntity(product, quantity));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact(DisplayName = $"The {nameof(PurchasableEntity)} can increase product quantity.")]
    [Trait("Category", "Unit")]
    public void CanIncreaseQuantity()
    {
        // Arrange
        var quantity = 1;
        var product = new Product(
            item: new Item(
                new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        var entity = new PurchasableEntity(product, quantity);
        var expectedResult = 3;

        // Act
        entity.IncQuantity();
        entity.IncQuantity();
        var result = entity.Quantity;
        entity.IncQuantity();
        entity.IncQuantity();
        entity.IncQuantity();
        entity.IncQuantity();

        // Assert
        result.Should().Be(expectedResult);
        entity.Quantity.Should().Be(product.Quantity);
    }

    [Fact(DisplayName = $"The {nameof(PurchasableEntity)} can decrease product quantity.")]
    [Trait("Category", "Unit")]
    public void CanDecreaseQuantity()
    {
        // Arrange
        var quantity = 4;
        var product = new Product(
            item: new Item(
                new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            provider: new Provider(
                new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            price: new Price(100m),
            quantity: 5);

        var entity = new PurchasableEntity(product, quantity);
        var expectedResult = 2;

        // Act
        entity.DecQuantity();
        entity.DecQuantity();
        var result = entity.Quantity;
        entity.DecQuantity();
        entity.DecQuantity();
        entity.DecQuantity();

        // Assert
        result.Should().Be(expectedResult);
        entity.Quantity.Should().Be(1);
    }
}
