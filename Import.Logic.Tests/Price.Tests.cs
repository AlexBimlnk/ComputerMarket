using Import.Logic.Models;

namespace Import.Logic.Tests;

public class PriceTests
{
    [Fact(DisplayName = $"The {nameof(Price)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Price? price = null!;

        // Act
        var exception = Record.Exception(() => price = new Price(100m));

        // Assert
        exception.Should().BeNull();
        price?.Value.Should().Be(100m);
    }

    [Fact(DisplayName = $"The {nameof(Price)} cannot be created wihtout parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Act
        var exception = Record.Exception(() => _ = new Price());

        // Assert
        exception.Should().BeOfType<NotSupportedException>();
    }

    [Theory(DisplayName = $"The {nameof(Price)} cannot be created when price less than or equal to zero.")]
    [Trait("Category", "Unit")]
    [InlineData(-0.00000001)]
    [InlineData(0)]
    [InlineData(-5)]
    public void CanNotCreateWhenPriceLessOne(decimal price)
    {
        // Act
        var exception = Record.Exception(() => _ = new Price(price));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}