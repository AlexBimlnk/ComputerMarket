using General.Logic.Executables;

using Market.Logic.Queries.Import;

namespace Market.Logic.Tests.Queries.Import;

public class GetLinksQueryTests
{
    [Fact(DisplayName = $"The {nameof(GetLinksQuery)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        GetLinksQuery command = null!;

        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() => command =
            new GetLinksQuery(id));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(GetLinksQuery)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new GetLinksQuery(id: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
