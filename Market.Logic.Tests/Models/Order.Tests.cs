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
        var user = TestHelper.GetOrdinaryUser();
        var entities = TestHelper.GetOrdinaryPurchasableEntities();

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
        var entities = TestHelper.GetOrdinaryPurchasableEntities();

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
        var user = TestHelper.GetOrdinaryUser();

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
        var user = TestHelper.GetOrdinaryUser();
        var entities = TestHelper.GetOrdinaryPurchasableEntities()
            .Select(x => 
            { 
                x.Selected = false; 
                return x; 
            })
            .ToHashSet();

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
        var user = TestHelper.GetOrdinaryUser();
        var entities = TestHelper.GetOrdinaryPurchasableEntities();

        var order = new Order(user, entities);
        var expectedResult = entities.Select(x => x.Product.FinalCost * x.Quantity).Sum();

        // Act
        var result = order.GetSumCost();

        // Assert
        result.Should().Be(expectedResult);
    }
}

