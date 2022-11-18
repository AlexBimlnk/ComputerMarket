using Market.Logic.Commands;
using Market.Logic.Commands.Import;
using Market.Logic.Models;

namespace Market.Logic.Tests.Commands;

public class DeleteLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        DeleteLinkCommand command = null!;

        var id = new CommandId("some id");

        var externalItemId = new ID(2);

        var provider = new Provider(
            new ID(1),
            "some name",
            new Margin(2),
            new PaymentTransactionsInformation(
                inn: "0123456789",
                bankAccount: "01234012340123401234"));

        // Act
        var exception = Record.Exception(() => command =
            new DeleteLinkCommand(
                id,
                externalItemId,
                provider));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
        command.ExternalItemId.Should().Be(externalItemId);
        command.Provider.Should().Be(provider);
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Arrange
        var externalItemId = new ID(2);

        var provider = new Provider(
            new ID(1),
            "some name",
            new Margin(2),
            new PaymentTransactionsInformation(
                inn: "0123456789",
                bankAccount: "01234012340123401234"));

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(
                id: null!,
                externalItemId,
                provider));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProvider()
    {
        // Arrange
        var id = new CommandId("some id");

        var externalItemId = new ID(2);

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(
                id,
                externalItemId,
                provider: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
