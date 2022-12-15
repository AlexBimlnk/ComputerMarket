using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class FilterValueTests
{
    [Fact(DisplayName = $"The {nameof(FilterValue)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        FilterValue value = null!;
        var stringValue = "value";
        var propertyId = new ID(1);

        // Act
        var exception = Record.Exception(() => value = new FilterValue(propertyId,stringValue));

        // Assert
        exception.Should().BeNull();
        value.Value.Should().Be(stringValue);
        value.Selected.Should().BeFalse();
        value.PropertyID.Should().Be(propertyId);
    }

    [Theory(DisplayName = $"The {nameof(FilterValue)} cannot be created with empty or null or white spaced value.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreatWhenNameIsBad(string value)
    {
        // Act
        var exception = Record.Exception(() => _ = new FilterValue(new ID(1), value));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
