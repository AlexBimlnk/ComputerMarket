using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class UpdateByProductTests
{
    [Fact(DisplayName = $"The {nameof(UpdateByProduct)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        UpdateByProduct updateByProduct = null!;
        var providerId = new ID(2); 
        var internalItemId = new ID(1);
        var externalItemId = new ID(1);
        var price = new Price(100m);
        var quantity = 5;

        // Act
        var exception = Record.Exception(() => updateByProduct = new UpdateByProduct(
            externalItemId,
            internalItemId,
            providerId,
            price,
            quantity));

        // Assert
        exception.Should().BeNull();
        updateByProduct.InternalID.Should().Be(internalItemId);
        updateByProduct.ExternalID.Should().Be(externalItemId);
        updateByProduct.ProviderID.Should().Be(providerId);
        updateByProduct.Quantity.Should().Be(quantity);
        updateByProduct.Price.Should().Be(price);
    }

    [Fact(DisplayName = $"The {nameof(UpdateByProduct)} cannot be created when quantity less 0.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenQuantityLessZero()
    {
        // Act
        var exception = Record.Exception(() => _ = new UpdateByProduct(
            externalID: new(1),
            internalID: new(1),
            providerID: new(1),
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}
