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

        // Act
        var exception = Record.Exception(() =>
            _ = new CommandFactory(setLink));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create command.")]
    [Trait("Category", "Unit")]
    public void CanCreateSetLinkCommand()
    {
        // Arrange
        var parameters = new SetLinkCommandParameters(
            new("some id"),
            new(1),
            new(1, Provider.Ivanov));

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var setLink = new Mock<Func<SetLinkCommandParameters, ICommand>>(MockBehavior.Strict);
        var setLinkInvokeCount = 0;
        setLink.Setup(x => x.Invoke(parameters))
            .Returns(command)
            .Callback(() => setLinkInvokeCount++);

        var factory = new CommandFactory(setLink.Object);

        // Act
        ICommand result = null!;
        var exception = Record.Exception(() =>
            result = factory.Create(parameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(command);
        setLinkInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Arrange
        var setLink = Mock.Of<Func<SetLinkCommandParameters, ICommand>>(MockBehavior.Strict);

        var factory = new CommandFactory(setLink);

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
        var unknownParameters = new UnknownParameters(new("some id"));

        var factory = new CommandFactory(setLink);

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
