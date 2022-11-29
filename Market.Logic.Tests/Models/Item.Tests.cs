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
        var type = TestHelper.GetOrdinaryItemType();
        var name = "name";
        var id = new ID(1);
        var properties = TestHelper.GetOrdinaryItemProperties();

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
        // Act
        var exception = Record.Exception(() => _ = new Item(
            new ID(1),
            type: null!,
            name: "name",
            TestHelper.GetOrdinaryItemProperties()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Item)} cannot be created without properties.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProperties()
    {
        // Act
        var exception = Record.Exception(() => _ = new Item(
            new ID(1),
            TestHelper.GetOrdinaryItemType(),
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
        // Act
        var exception = Record.Exception(() => _ = new Item(
            new ID(1),
            TestHelper.GetOrdinaryItemType(),
            name,
            TestHelper.GetOrdinaryItemProperties()));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
