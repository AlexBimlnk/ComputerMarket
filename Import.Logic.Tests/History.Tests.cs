using Import.Logic.Models;

namespace Import.Logic.Tests;
public class HistoryTests
{
    [Fact(DisplayName = $"The {nameof(History)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);
        var productName = "Some name";
        var description = "Some description";

        History? history = null!;

        // Act
        var exception = Record.Exception(() => history = new History(
            externalId,
            productName,
            description));

        // Assert
        exception.Should().BeNull();
        history.ExternalId.Should().Be(externalId);
        history.ProductName.Should().Be(productName);
        history.ProductDescription.Should().Be(description);
    }

    [Fact(DisplayName = $"The {nameof(History)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutDescription()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);
        var productName = "Some name";

        History? history = null!;

        // Act
        var exception = Record.Exception(() => history = new History(
            externalId,
            productName));

        // Assert
        exception.Should().BeNull();
        history.ExternalId.Should().Be(externalId);
        history.ProductName.Should().Be(productName);
        history.ProductDescription.Should().Be(null);
    }

    [Theory(DisplayName = $"The {nameof(History)} can't be created when product name is null, empty " +
        $"or has only whitespaces.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(" \n\r \t ")]
    public void CanNotBeCreatedWhenGivenBadProductName(string productName)
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);

        History? history = null!;

        // Act
        var exception = Record.Exception(() => history = new History(
            externalId,
            productName));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
