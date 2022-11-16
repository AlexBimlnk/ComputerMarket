using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class BasketTests
{
    [Fact(DisplayName = $"The {nameof(Basket)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Basket Basket = null!;

        // Act
        var exception = Record.Exception(() => Basket = new Basket());

        // Assert
        exception.Should().BeNull();
        Basket.Items.Should().NotBeNull();
        Basket.Items.Any().Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(Basket)} can add {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanAddProduct()
    {
        // Arrange
        var basket = new Basket();
        var product = new Product(
            new Item(
                id: new ID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new ID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        // Act
        basket.Add(product);

        // Assert
        basket.Items.Count.Should().Be(1);
        basket.Items.Where(x => x.Product.Equals(product)).FirstOrDefault().Should().NotBeNull();
        basket.Items.Where(x => x.Product.Equals(product)).First().Quantity.Should().Be(1);
        basket.Items.Where(x => x.Product.Equals(product)).First().Selected.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(Basket)} can't add null {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanNotAddNullProduct()
    {
        // Arrange       
        var basket = new Basket();

        // Act
        var exception = Record.Exception(() => basket.Add(product: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Basket)} can get item by product.")]
    [Trait("Category", "Unit")]
    public void CanGetBasketItem()
    {
        // Arrange
        var quantity = 1;
        var product = new Product(
            new Item(
                id: new ID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new ID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        var basket = new Basket();
        basket.Add(product);

        var expectedResult = new PurchasableEntity(product, quantity);

        // Act
        var result = basket.GetBasketItem(product);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        result.Quantity.Should().Be(quantity);
        result.Product.Should().BeEquivalentTo(product);
    }

    [Fact(DisplayName = $"The {nameof(Basket)} can't get item by null {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanNotGetItemByNullProduct()
    {
        // Arrange       
        var basket = new Basket();

        // Act
        var exception = Record.Exception(() => basket.GetBasketItem(product: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Basket)} can't get item of doesn't exsisting product.")]
    [Trait("Category", "Unit")]
    public void CanNotGetItemOfNotExsisingProduct()
    {
        // Arrange       
        var basket = new Basket();
        var product = new Product(
            new Item(
                id: new ID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new ID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        // Act
        var exception = Record.Exception(() => basket.GetBasketItem(product));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }


    [Fact(DisplayName = $"The {nameof(Basket)} increase quantity when add same products.")]
    [Trait("Category", "Unit")]
    public void CanIncQuantityWhenAddSameProducts()
    {
        // Arrange
        var item = new Item(
            id: new ID(1),
            new ItemType("some_type"),
            "some_name",
            properties: Array.Empty<ItemProperty>());

        var prodvider = new Provider(
            id: new ID(1),
            "provider_name",
            new Margin(1.1m),
            new PaymentTransactionsInformation(
                inn: "0123456789",
                bankAccount: "01234012340123401234"));

        var product1 = new Product(item, prodvider, new Price(100m), 5);
        var product2 = new Product(item, prodvider, new Price(100m), 5);

        var basket = new Basket();

        // Act
        basket.Add(product1);
        basket.Add(product2);

        var result1 = basket.GetBasketItem(product1);
        var result2 = basket.GetBasketItem(product2);

        // Assert
        basket.Items.Count.Should().Be(1);
        result1.Quantity.Should().Be(2);
        result1.Selected.Should().BeTrue();
        result2.Quantity.Should().Be(2);
        result2.Selected.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(Basket)} can remove {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanRemoveProduct()
    {
        // Arrange
        var product = new Product(
            new Item(
                id: new ID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new ID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        var basket = new Basket();
        basket.Add(product);

        // Act
        basket.Remove(product);

        // Assert
        basket.Items.Any().Should().BeFalse();
    }
}

