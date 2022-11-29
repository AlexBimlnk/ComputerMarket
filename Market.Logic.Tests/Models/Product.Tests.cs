using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ProductTests
{
    [Fact(DisplayName = $"The {nameof(Product)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var providerId = new ID(2); 
        var itemId = new ID(1);
        Product product = null!;
        var provider = TestHelper.GetOrdinaryProvider(2);

        var item = TestHelper.GetOrdinaryItem(1);

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
        product.FinalCost.Should().Be(100m * provider.Margin.Value);
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created when quantity less 0.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenQuantityLessZero()
    {
        // Act
        var exception = Record.Exception(() => _ = new Product(
            TestHelper.GetOrdinaryItem(),
            TestHelper.GetOrdinaryProvider(),
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created without item.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutItem()
    {
        // Act
        var exception = Record.Exception(() => _ = new Product(
            item: null!,
            TestHelper.GetOrdinaryProvider(),
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created without provider.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProvider()
    {
        // Act
        var exception = Record.Exception(() => _ = new Product(
            TestHelper.GetOrdinaryItem(),
            provider: null!,
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
