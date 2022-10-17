using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ItemTypeTests
{
    [Fact(DisplayName = $"The {nameof(ItemType)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var name = "name";
        ItemType property = null!;

        // Act
        var exception = Record.Exception(() => property = new ItemType(name));

        // Assert
        exception.Should().BeNull();
        property.Name.Should().Be(name);
    }

    [Theory(DisplayName = $"The {nameof(ItemType)} cannot be created with empty or null or white spaced name.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreatWhenNameIsBad(string name)
    {
        // Act
        var exception = Record.Exception(() => _ = new ItemType(name));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
