﻿using General.Logic.Executables;

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

        var id = new ExecutableID("some id");

        var internalItemId = new ID(1);
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
        var internalItemId = new ID(1);
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
        var id = new ExecutableID("some id");

        var internalItemId = new ID(1);
        var externalItemId = new ID(2);

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
