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
            id: new InternalID(1),
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);
        var quantity = 2;
        var product = new Product(
            new Item(
                id: new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        var entities = new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, quantity)
        };

        // Act
        var exception = Record.Exception(() => order = new Order(user, entities));

        // Assert
        exception.Should().BeNull();
        order.Creator.Should().Be(user);
        order.State.Should().Be(OrderState.PaymentWait);
        order.Items.Should().BeEquivalentTo(entities, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Order)} cannot be created without {nameof(User)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutUser()
    {
        // Arrange
        var quantity = 2;
        var product = new Product(
            new Item(
                id: new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        var entities = new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, quantity)
        };

        // Act
        var exception = Record.Exception(() => _ = new Order(user: null!, entities));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Order)} cannot be created without items.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutItems()
    {
        // Arrange       
        var user = new User(
            id: new InternalID(1),
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);

        // Act
        var exception = Record.Exception(() => _ = new Order(user, entities: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }


    [Fact(DisplayName = $"The {nameof(Order)} cannot be created with zero items.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithZeroItems()
    {
        // Arrange       
        var user = new User(
            id: new InternalID(1),
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);
        var quantity = 2;
        var product = new Product(
            new Item(
                id: new InternalID(1),
                new ItemType("some_type"),
                "some_name",
                properties: Array.Empty<ItemProperty>()),
            new Provider(
                id: new InternalID(1),
                "provider_name",
                new Margin(1.1m),
                new PaymentTransactionsInformation(
                    "0123456789",
                    "01234012340123401234")),
            new Price(100m),
            quantity: 10);

        var entities = new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, quantity)
            {
                Selected = false
            }
        }; 

        // Act
        var exception = Record.Exception(() => _ = new Order(user, entities));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Order)} can calculate order sum cost.")]
    [Trait("Category", "Unit")]
    public void CanCalculateOrderSumCost()
    {
        // Arrange
        var user = new User(
            id: new InternalID(1),
            login: "login",
            new Password("12345"),
            email: "mail@mail.ru",
            UserType.Customer);
        var provider1 = new Provider(
            id: new InternalID(1),
            "provider_name",
            new Margin(1.1m),
            new PaymentTransactionsInformation(
                inn: "0123456789",
                bankAccount: "01234012340123401234"));
        var provider2 = new Provider(
            id: new InternalID(2),
            "provider_name",
            new Margin(1.6m),
            new PaymentTransactionsInformation(
                inn: "0123456789", 
                bankAccount:"01234012340123401234"));
        var quantity1 = 2;
        var quantity2 = 4;
        var price1 = 100m;
        var price2 = 400m;
        var item1 = new Item(
            id: new InternalID(1),
            new ItemType("some_type"),
            "some_name1",
            properties: Array.Empty<ItemProperty>());
        var item2 = new Item(
            id: new InternalID(2),
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

        var entities = new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product1, quantity1),
            new PurchasableEntity(product2, quantity2)

        };
       
        var order = new Order(user, entities);
        var expectedResult = quantity1 * provider1.Margin.Value * price1 + quantity2 * provider2.Margin.Value * price2;

        // Act
        var result = order.GetSumCost();

        // Assert
        result.Should().Be(expectedResult);
    }
}

