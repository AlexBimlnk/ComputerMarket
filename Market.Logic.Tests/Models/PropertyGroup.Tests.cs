using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class PropertyGroupTests
{
    [Fact(DisplayName = $"The {nameof(PropertyGroup)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var name = "name";
        var id = new ID(1);
        PropertyGroup property = null!;

        // Act
        var exception = Record.Exception(() => property = new PropertyGroup(id, name));

        // Assert
        exception.Should().BeNull();
        property.Name.Should().Be(name);
        property.Id.Should().Be(id);
    }

    [Theory(DisplayName = $"The {nameof(PropertyGroup)} cannot be created with empty or null or white spaced name.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreatWhenNameIsBad(string name)
    {
        // Act
        var exception = Record.Exception(() => _ = new PropertyGroup(new ID(1), name));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(PropertyGroup)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanCreateDefaultGroup()
    {
        // Arrange
        PropertyGroup property = null!;

        // Act
        var exception = Record.Exception(() => property = PropertyGroup.Default);

        // Assert
        exception.Should().BeNull();
        property.Name.Should().Be("None");
        property.Id.Should().Be(new ID(-1));
    }
}
