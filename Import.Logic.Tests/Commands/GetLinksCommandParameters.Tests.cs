using General.Logic.Commands;
using General.Logic.Executables;

using Import.Logic.Models;
using Import.Logic.Queries;

namespace Import.Logic.Tests.Commands;
public class GetLinksCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(GetLinksQueryParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        GetLinksQueryParameters parameters = null!;
        var id = new ExecutableID("some id");
        var internalID = new InternalID(1);
        var externalID = new ExternalID(1, Provider.Ivanov);

        // Act
        var exception = Record.Exception(() => parameters = new GetLinksQueryParameters(id));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(GetLinksQueryParameters)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Act
        var exception = Record.Exception(() => _ = new GetLinksQueryParameters(id: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
