using Market.Logic.ComputerBuilder;
using Market.Logic.Models;

namespace Market.Logic.Tests;
public class ComputerBuildRuleTests
{
    [Fact(DisplayName = $"The {nameof(ComputerBuildRule)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        ComputerBuildRule rule = null!;
        var description = "some description";
        var forType = (ItemTypeID: new ID(1), PropertyID: new ID(2));
        var compareBy = (ItemTypeID: new ID(4), PropertyID: new ID(2));
        var func = new Func<ItemProperty, ItemProperty, bool>((motherBoardSocket, cpuSocket) => motherBoardSocket.Value == cpuSocket.Value);

        // Act
        var exception = Record.Exception(() => rule = new ComputerBuildRule(
            description,
            forType,
            compareBy,
            func));

        // Assert
        exception.Should().BeNull();
        rule.Description.Should().Be(description);
        rule.ForType.Should().Be(forType);
        rule.CompareBy.Should().Be(compareBy);
        rule.CompareFunction.Should().Be(func);
    }

    [Theory(DisplayName = $"The {nameof(ComputerBuildRule)} can't be create with bad description.")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData(" \r \n \t  ")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithBadDescription(string description)
    {
        // Arrange
        var forType = (ItemTypeID: new ID(1), PropertyID: new ID(2));
        var compareBy = (ItemTypeID: new ID(4), PropertyID: new ID(2));
        var func = new Func<ItemProperty, ItemProperty, bool>((motherBoardSocket, cpuSocket) => motherBoardSocket.Value == cpuSocket.Value);

        // Act
        var exception = Record.Exception(() => _ = new ComputerBuildRule(
            description,
            forType,
            compareBy,
            func));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(ComputerBuildRule)} can't create when item type ids be same.")]
    [Trait("Category", "Unit")]
    public void CanNotCreatedWhenItemTypeIdsBeSame()
    {
        // Arrange
        var description = "some description";
        var forType = (ItemTypeID: new ID(1), PropertyID: new ID(2));
        var compareBy = (ItemTypeID: new ID(1), PropertyID: new ID(2));
        var func = new Func<ItemProperty, ItemProperty, bool>((motherBoardSocket, cpuSocket) => motherBoardSocket.Value == cpuSocket.Value);

        // Act
        var exception = Record.Exception(() => _ = new ComputerBuildRule(
            description,
            forType,
            compareBy,
            func));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(ComputerBuildRule)} can't create without compare function.")]
    [Trait("Category", "Unit")]
    public void CanNotCreatedWithoutCompareFunction()
    {
        // Arrange
        var description = "some description";
        var forType = (ItemTypeID: new ID(1), PropertyID: new ID(2));
        var compareBy = (ItemTypeID: new ID(4), PropertyID: new ID(2));

        // Act
        var exception = Record.Exception(() => _ = new ComputerBuildRule(
            description,
            forType,
            compareBy,
            func: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
