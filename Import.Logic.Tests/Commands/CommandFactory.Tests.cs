using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;
using Import.Logic.Models;

using Moq;

namespace Import.Logic.Tests.Commands;
public class CommandFactoryTests
{
    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var setLink = Mock.Of<Func<SetLinkCommandParameters, ICommand>>();
        var deleteLink = Mock.Of<Func<DeleteLinkCommandParameters, ICommand>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CommandFactory(setLink, deleteLink));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create set link command.")]
    [Trait("Category", "Unit")]
    public void CanCreateSetLinkCommand()
    {
        // Arrange
        var setParameters = new SetLinkCommandParameters(
            new("some id"),
            new(1),
            new(1, Provider.Ivanov));

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var setLink = new Mock<Func<SetLinkCommandParameters, ICommand>>(MockBehavior.Strict);
        var deleteLink = Mock.Of<Func<DeleteLinkCommandParameters, ICommand>>();

        var setLinkInvokeCount = 0;
        setLink.Setup(x => x.Invoke(setParameters))
            .Returns(command)
            .Callback(() => setLinkInvokeCount++);
        
        var factory = new CommandFactory(setLink.Object, deleteLink);

        // Act
        ICommand result = null!;
        
        var exception = Record.Exception(() =>
            result = factory.Create(setParameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(command);
        setLinkInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create delete link command.")]
    [Trait("Category", "Unit")]
    public void CanCreateDeleteLinkCommand()
    {
        // Arrange
        var deleteParameters = new DeleteLinkCommandParameters(
            new("some id"),
            new(1, Provider.Ivanov));

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var setLink = Mock.Of<Func<SetLinkCommandParameters, ICommand>>();
        var deleteLink = new Mock<Func<DeleteLinkCommandParameters, ICommand>>(MockBehavior.Strict);
        
        var deleteLinkInvokeCount = 0;
        deleteLink.Setup(x => x.Invoke(deleteParameters))
            .Returns(command)
            .Callback(() => deleteLinkInvokeCount++);


        var factory = new CommandFactory(setLink, deleteLink.Object);

        // Act
        ICommand result = null!;

        var exception = Record.Exception(() =>
            result = factory.Create(deleteParameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(command);
        deleteLinkInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Arrange
        var setLink = Mock.Of<Func<SetLinkCommandParameters, ICommand>>(MockBehavior.Strict);
        var deleteLink = Mock.Of<Func<DeleteLinkCommandParameters, ICommand>>();

        var factory = new CommandFactory(setLink, deleteLink);

        // Act
        var exception = Record.Exception(() =>
            _ = factory.Create(parameters: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can't create with unknown parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithUnknownParameters()
    {
        // Arrange
        var setLink = Mock.Of<Func<SetLinkCommandParameters, ICommand>>(MockBehavior.Strict);
        var deleteLink = Mock.Of<Func<DeleteLinkCommandParameters, ICommand>>();
        var unknownParameters = new UnknownParameters(new("some id"));

        var factory = new CommandFactory(setLink, deleteLink);

        // Act
        var exception = Record.Exception(() =>
            _ = factory.Create(unknownParameters));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    private class UnknownParameters : CommandParametersBase
    {
        public UnknownParameters(CommandID id) : base(id)
        {
        }
    }
}
