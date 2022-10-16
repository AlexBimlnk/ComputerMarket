using Market.Logic.Models;

namespace Market.Logic.Tests.Models;
public class ItemPropertyTests
{
    [Fact(DisplayName = $"The {nameof(ItemProperty)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var name = "name";
        var value = "value";
        ItemProperty property = null!;

        // Act
        var exception = Record.Exception(() => property = new ItemProperty(name, value));

        // Assert
        exception.Should().BeNull();
        property.Name.Should().Be(name);
        property.Value.Should().Be(value);
    }

    [Theory(DisplayName = $"The {nameof(ItemProperty)} cannot be created with empty or null or white spaced name.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreatWhenNameIsBad(string name)
    {
        // Act
        var exception = Record.Exception(() => _ = new ItemProperty(name, value: "value"));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(ItemProperty)} cannot be created with empty or null or white spaced value.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreateWhenValueIsBad(string value)
    {
        // Act
        var exception = Record.Exception(() => _ = new ItemProperty(name: "name", value));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}