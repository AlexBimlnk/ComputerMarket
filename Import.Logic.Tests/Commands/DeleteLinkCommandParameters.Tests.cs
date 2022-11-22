using General.Logic.Commands;
using General.Logic.Executables;

using Import.Logic.Commands;
using Import.Logic.Models;

namespace Import.Logic.Tests.Commands;

public class DeleteLinkCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(DeleteLinkCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        DeleteLinkCommandParameters parameters = null!;
        var id = new ExecutableID("some id");
        var internalID = new InternalID(1);
        var externalID = new ExternalID(1, Provider.Ivanov);

        // Act
        var exception = Record.Exception(() => parameters = new DeleteLinkCommandParameters(
            id,
            externalID));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(id);
        parameters.ExternalID.Should().Be(externalID);
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommandParameters)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Act
        var exception = Record.Exception(() => _ = new DeleteLinkCommandParameters(
            id: null!,
            externalID: new(1, Provider.Ivanov)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}

