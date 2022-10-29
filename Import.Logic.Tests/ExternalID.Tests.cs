using Import.Logic.Models;

namespace Import.Logic.Tests;

public class ExternalIDTests
{
    [Fact(DisplayName = $"The {nameof(ExternalID)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        ExternalID? externalId = null!;

        // Act
        var exception = Record.Exception(() => 
            externalId = new ExternalID(100, Provider.Ivanov));

        // Assert
        exception.Should().BeNull();
        externalId?.Value.Should().Be(100);
        externalId?.Provider.Should().Be(Provider.Ivanov);
    }

    [Fact(DisplayName = $"The {nameof(ExternalID)} cannot be created wihtout parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Act
        var exception = Record.Exception(() => _ = new ExternalID());

        // Assert
        exception.Should().BeOfType<NotSupportedException>();
    }

    [Theory(DisplayName = $"The {nameof(ExternalID)} cannot be created when provider is not defined.")]
    [Trait("Category", "Unit")]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1000000)]
    public void CanNotCreateWhenPriceLessOne(long provider)
    {
        // Act
        var exception = Record.Exception(() => 
            _ = new ExternalID(1, (Provider)provider));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}