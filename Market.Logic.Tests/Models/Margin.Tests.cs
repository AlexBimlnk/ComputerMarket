using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class MarginTests
{
    [Fact(DisplayName = $"The {nameof(Margin)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Margin? Margin = null!;

        // Act
        var exception = Record.Exception(() => Margin = new Margin(1.2m));

        // Assert
        exception.Should().BeNull();
        Margin?.Value.Should().Be(1.2m);
    }

    [Fact(DisplayName = $"The {nameof(Margin)} cannot be created wihtout parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Act
        var exception = Record.Exception(() => _ = new Margin());

        // Assert
        exception.Should().BeOfType<NotSupportedException>();
    }

    [Theory(DisplayName = $"The {nameof(Margin)} cannot be created when margin value is out of range [1; 1000].")]
    [Trait("Category", "Unit")]
    [InlineData(-0.00000001)]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(100000000)]
    [InlineData(0.9999999999)]
    public void CanNotCreateWhenMarginLessOne(decimal value)
    {
        // Act
        var exception = Record.Exception(() => _ = new Margin(value));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}

