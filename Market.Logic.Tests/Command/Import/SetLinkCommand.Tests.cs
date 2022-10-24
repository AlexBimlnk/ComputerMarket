using Market.Logic.Commands;
using Market.Logic.Commands.Import;
using Market.Logic.Models;

namespace Market.Logic.Tests.Commands;

public class SetLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        SetLinkCommand command = null!;

        var id = new CommandId("some id");

        var internalItemId = new InternalID(1);
        var externalItemId = new InternalID(2);

        var provider = new Provider(
            "some name",
            new Margin(1),
            paymentTransactionsInformation: new("2313213", "13123123"));

        // Act
        var exception = Record.Exception(() => command =
            new SetLinkCommand(
                id,
                internalItemId,
                externalItemId,
                provider));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
        command.InternalItemId.Should().Be(internalItemId);
        command.ExternalItemId.Should().Be(externalItemId);
        command.Provider.Should().Be(provider);
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Arrange
        var internalItemId = new InternalID(1);
        var externalItemId = new InternalID(2);

        var provider = new Provider(
            "some name",
            new Margin(1),
            paymentTransactionsInformation: new("2313213", "13123123"));

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(
                id: null!,
                internalItemId,
                externalItemId,
                provider));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProvider()
    {
        // Arrange
        var id = new CommandId("some id");

        var internalItemId = new InternalID(1);
        var externalItemId = new InternalID(2);

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(
                id,
                internalItemId,
                externalItemId,
                provider: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
