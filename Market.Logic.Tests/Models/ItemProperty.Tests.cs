using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ItemPropertyTests
{
    [Fact(DisplayName = $"The {nameof(ItemProperty)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var id = new ID(1);
        var name = "name";
        ItemProperty property = null!;
        var group = TestHelper.GetOrdinaryPropertyGroup();
        var isFilterable = false;
        var dataType = PropertyDataType.String;

        // Act
        var exception = Record.Exception(() => property = new ItemProperty(id, name, group, isFilterable, dataType));

        // Assert
        exception.Should().BeNull();
        property.Key.Should().Be(id);
        property.Name.Should().Be(name);
        property.Group.Should().Be(group);
        property.IsFilterable.Should().Be(isFilterable);
        property.ProperyDataType.Should().Be(dataType);
        property.Value.Should().Be(null);
    }

    [Fact(DisplayName = $"The {nameof(ItemProperty)} cannot be created without group.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutGroup()
    {
        // Act
        var exception = Record.Exception(() => _ = new ItemProperty(
            new ID(1), 
            name: "name", 
            group: null!, 
            isFilterable: false,
            PropertyDataType.String));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ItemProperty)} cannot be created with invalid enum value.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithInvalidEnumValue()
    {
        // Act
        var exception = Record.Exception(() => _ = new ItemProperty(
            new ID(1),
            name: "name",
            TestHelper.GetOrdinaryPropertyGroup(),
            isFilterable: false,
            (PropertyDataType)10));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
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
        var exception = Record.Exception(() => _ = new ItemProperty(
            new ID(1), 
            name, 
            TestHelper.GetOrdinaryPropertyGroup(), 
            isFilterable: false, 
            PropertyDataType.String));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(ItemProperty)} can set value.")]
    [Trait("Category", "Unit")]
    public void CanSetValue()
    {
        // Arrange
        var value = "value";
        var property = TestHelper.GetOrdinaryItemProperty();

        // Act
        var exception = Record.Exception(() => property.Value = value);

        // Assert
        exception.Should().BeNull();
        property.Value.Should().Be(value);
    }

    [Theory(DisplayName = $"The {nameof(ItemProperty)} cannot set with empty or null or white spaced value.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n ")]
    [InlineData("")]
    public void CanNotCreateWhenValueIsBad(string value)
    {
        // Arrange
        var property = TestHelper.GetOrdinaryItemProperty();

        // Act
        var exception = Record.Exception(() => property.Value = value);

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}