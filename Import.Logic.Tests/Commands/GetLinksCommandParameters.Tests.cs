using General.Logic.Commands;

using Import.Logic.Commands;
using Import.Logic.Models;

namespace Import.Logic.Tests.Commands;
public class GetLinksCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(GetLinksCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        GetLinksCommandParameters parameters = null!;
        var id = new CommandID("some id");
        var internalID = new InternalID(1);
        var externalID = new ExternalID(1, Provider.Ivanov);

        // Act
        var exception = Record.Exception(() => parameters = new GetLinksCommandParameters(id));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(GetLinksCommandParameters)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Act
        var exception = Record.Exception(() => _ = new GetLinksCommandParameters(id: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
