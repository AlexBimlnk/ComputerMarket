using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class OrderTests
{
    [Fact(DisplayName = $"The {nameof(Order)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Order order = null!;
        var user = new User(
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);
        var quantity = 2;
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
            quantity: 10);

        var products = new Dictionary<Product, int>();
        products[product] = quantity;

        var expectedOrderItems = products.Select(x => new OrderItem(x.Key, x.Value));

        // Act
        var exception = Record.Exception(() => order = new Order(user, products));

        // Assert
        exception.Should().BeNull();
        order.Creator.Should().Be(user);
        order.State.Should().Be(OrderState.PaymentWait);
        order.Items.Should().BeEquivalentTo(expectedOrderItems, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Order)} cannot be created without {nameof(User)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutUser()
    {
        // Arrange
        var quantity = 2;
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
            quantity: 10);
        var dictionary = new Dictionary<Product, int>();
        dictionary[product] = quantity;

        // Act
        var exception = Record.Exception(() => _ = new Order(user: null!, dictionary));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Order)} cannot be created without {nameof(Product)}s.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProducts()
    {
        // Arrange       
        var user = new User(
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);

        // Act
        var exception = Record.Exception(() => _ = new Order(user, products: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }


    [Fact(DisplayName = $"The {nameof(Order)} cannot be created with zero products.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithZeroProducts()
    {
        // Arrange       
        var user = new User(
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);

        var products = new Dictionary<Product, int>();

        // Act
        var exception = Record.Exception(() => _ = new Order(user, products));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Order)} can calculate order sum cost.")]
    [Trait("Category", "Unit")]
    public void CanCalculateOrderSumCost()
    {
        // Arrange
        var user = new User(
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);
        var provider1 = new Provider(
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234"));
        var provider2 = new Provider(
                "provider_name",
                new Margin(1.6m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234"));
        var quantity1 = 2;
        var quantity2 = 4;
        var price1 = 100m;
        var price2 = 400m;
        var item1 = new Item(
                new ItemType("some_type"),
                "some_name1",
                properties: Array.Empty<ItemProperty>());
        var item2 = new Item(
                new ItemType("some_type"),
                "some_name2",
                properties: Array.Empty<ItemProperty>());

        var product1 = new Product(
            item1,
            provider1,
            price: new Price(price1),
            quantity: 10);

        var product2 = new Product(
            item2,
            provider2,
            price: new Price(price2),
            quantity: 10);

        var products = new Dictionary<Product, int>();
        products[product1] = quantity1;
        products[product2] = quantity2;
        var order = new Order(user, products);
        var expectedResult = quantity1 * provider1.Margin.Value * price1 + quantity2 * provider2.Margin.Value * price2;

        // Act
        var result = order.GetSumCost();

        // Assert
        result.Should().Be(expectedResult);
    }
}

