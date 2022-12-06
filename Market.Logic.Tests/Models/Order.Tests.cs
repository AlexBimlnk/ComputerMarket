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
        var id = new ID(1);
        var user = TestHelper.GetOrdinaryUser();
        var entities = new PurchasableEntity[]
        {
            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        1, 
                        name: "Item1"), 
                    price: 20m, 
                    quantity: 5),
                1),
            
            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        2, 
                        name: "Item2"), 
                    price: 60m, 
                    quantity: 10),
                3)
        }.ToHashSet();

        // Act
        var exception = Record.Exception(() => order = new Order(id, user, entities));

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
        var entities = new PurchasableEntity[]
{
            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        1,
                        name: "Item1"),
                    price: 20m,
                    quantity: 5),
                1),

            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        2,
                        name: "Item2"),
                    price: 60m,
                    quantity: 10),
                3)
        }.ToHashSet();

        // Act
        var exception = Record.Exception(() => _ = new Order(new ID(1), user: null!, entities));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Order)} cannot be created without items.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutItems()
    {
        // Arrange       
        var user = TestHelper.GetOrdinaryUser();

        // Act
        var exception = Record.Exception(() => _ = new Order(new ID(1), user, entities: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }


    [Fact(DisplayName = $"The {nameof(Order)} cannot be created with zero items.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithZeroItems()
    {
        // Arrange       
        var user = TestHelper.GetOrdinaryUser();
        var entities = new PurchasableEntity[]
        {
            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        1,
                        name: "Item1"),
                    price: 20m,
                    quantity: 5),
                1),

            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        2,
                        name: "Item2"),
                    price: 60m,
                    quantity: 10),
                3)
        }
        .Select(x => 
        { 
            x.Selected = false; 
            return x; 
        })
        .ToHashSet();

        // Act
        var exception = Record.Exception(() => _ = new Order(new ID(1), user, entities));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Order)} can calculate order sum cost.")]
    [Trait("Category", "Unit")]
    public void CanCalculateOrderSumCost()
    {
        // Arrange
        var user = TestHelper.GetOrdinaryUser();
        var entities = new PurchasableEntity[]
{
            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        1,
                        name: "Item1"),
                    price: 20m,
                    quantity: 5),
                1),

            TestHelper.GetOrdinaryPurchasableEntity(
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(
                        2,
                        name: "Item2"),
                    price: 60m,
                    quantity: 10),
                3)
        }.ToHashSet();

        var order = new Order(new ID(1), user, entities);
        var expectedResult = entities.Select(x => x.Product.FinalCost * x.Quantity).Sum();

        // Act
        var result = order.GetSumCost();

        // Assert
        result.Should().Be(expectedResult);
    }
}

