using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ItemTests
{
    [Fact(DisplayName = $"The {nameof(Item)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Item item = null!;
        var type = new ItemType("some_type");
        var name = "name";
        var id = new InternalID(1);
        var properties = new ItemProperty[]
        {
            new ItemProperty("Prop1", "Val1"),
            new ItemProperty("Prop2", "Val2"),
            new ItemProperty("Prop3", "Val3")
        };

        // Act
        var exception = Record.Exception(() => item = new Item(
            id,
            type,
            name,
            properties));

        // Assert
        exception.Should().BeNull();
        item.Key.Should().Be(id);
        item.Type.Should().Be(type);
        item.Name.Should().Be(name);
        item.Properties.Should().BeEquivalentTo(properties, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Item)} cannot be created without type.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutItemType()
    {
        // Arrange
        var properties = new ItemProperty[]
        {
            new ItemProperty("Prop1", "Val1"),
            new ItemProperty("Prop2", "Val2"),
            new ItemProperty("Prop3", "Val3")
        };

        // Act
        var exception = Record.Exception(() => _ = new Item(
            new InternalID(1),
            type: null!,
            name: "name",
            properties));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Item)} cannot be created without properties.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProperties()
    {
        // Act
        var exception = Record.Exception(() => _ = new Item(
            new InternalID(1),
            type: new ItemType("some_type"),
            name: "name",
            properties: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(ItemProperty)} cannot be created with empty or null or white spaced value.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreateWhenItemNameIsBad(string name)
    {
        // Arrange
        var properties = new ItemProperty[]
        {
            new ItemProperty("Prop1", "Val1"),
            new ItemProperty("Prop2", "Val2"),
            new ItemProperty("Prop3", "Val3")
        };

        // Act
        var exception = Record.Exception(() => _ = new Item(
            new InternalID(1),
            type: new ItemType("some_type"),
            name,
            properties));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
