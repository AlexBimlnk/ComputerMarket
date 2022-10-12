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
        var metadata = "Some metadata";

        History? history = null!;

        // Act
        var exception = Record.Exception(() => history = new History(
            externalId,
            metadata));

        // Assert
        exception.Should().BeNull();
        history.ExternalId.Should().Be(externalId);
        history.ProductMetadata.Should().Be(metadata);
    }

    [Fact(DisplayName = $"The {nameof(History)} can be created without product metadata.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutProductMetadata()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);

        History? history = null!;

        // Act
        var exception = Record.Exception(() => history = new History(
            externalId,
            null!));

        // Assert
        exception.Should().BeNull();
        history.ExternalId.Should().Be(externalId);
        history.ProductMetadata.Should().BeNull();
    }
}
