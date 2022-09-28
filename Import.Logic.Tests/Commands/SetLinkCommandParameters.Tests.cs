using Import.Logic.Commands;
using Import.Logic.Models;

namespace Import.Logic.Tests.Commands;
public class SetLinkCommandParametersTests
{
    [Fact(DisplayName = $"The {nameof(SetLinkCommandParameters)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        SetLinkCommandParameters parameters = null!;
        var id = new CommandID("some id");
        var internalID = new InternalID(1);
        var externalID = new ExternalID(1, Provider.Ivanov);

        // Act
        var exception = Record.Exception(() => parameters = new SetLinkCommandParameters(
            id,
            internalID,
            externalID));

        // Assert
        exception.Should().BeNull();
        parameters.Id.Should().Be(id);
        parameters.InternalID.Should().Be(internalID);
        parameters.ExternalID.Should().Be(externalID);
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommandParameters)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Act
        var exception = Record.Exception(() => _ = new SetLinkCommandParameters(
            id: null!,
            internalID: new(1),
            externalID: new(1, Provider.Ivanov)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
