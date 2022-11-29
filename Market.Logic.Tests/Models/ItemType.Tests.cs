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
        var id = 1;
        ItemType property = null!;

        // Act
        var exception = Record.Exception(() => property = new ItemType(id, name));

        // Assert
        exception.Should().BeNull();
        property.Name.Should().Be(name);
        property.Id.Should().Be(id);
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
        var exception = Record.Exception(() => _ = new ItemType(id: 1, name));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(ItemType)} cannot be created with out ranged id.")]
    [Trait("Category", "Unit")]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-5000)]
    public void CanNotCreatWhenIdIsOutOfRange(int id)
    {
        // Act
        var exception = Record.Exception(() => _ = new ItemType(id, name: "Name"));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}
