﻿using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;

using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests;
public class HTTPCommandHandlerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HTTPCommandHandler>>(MockBehavior.Strict);
        var factory = Mock.Of<ICommandFactory>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, CommandParametersBase>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HTTPCommandHandler(logger, factory, deserializer));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var factory = Mock.Of<ICommandFactory>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, CommandParametersBase>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HTTPCommandHandler(logger: null!, factory, deserializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without command factory.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCommandFactory()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HTTPCommandHandler>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, CommandParametersBase>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HTTPCommandHandler(logger, commandFactory: null!, deserializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without deserializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutDeserializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HTTPCommandHandler>>(MockBehavior.Strict);
        var factory = Mock.Of<ICommandFactory>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HTTPCommandHandler(logger, factory, deserializer: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can handle command.")]
    [Trait("Category", "Unit")]
    public async Task CanHandleCommandAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HTTPCommandHandler>>();
        var factory = new Mock<ICommandFactory>(MockBehavior.Strict);
        var deserializer = new Mock<IDeserializer<string, CommandParametersBase>>(MockBehavior.Strict);

        var request = "Some requst";
        var commandId = new CommandID("some id");

        var commandParameters = new Mock<CommandParametersBase>(MockBehavior.Strict, commandId);

        var commandResult = CommandResult.Success(commandId);

        deserializer.Setup(x => x.Deserialize(request))
            .Returns(commandParameters.Object);

        var command = new Mock<ICommand>(MockBehavior.Strict);
        var commandInvokeCount = 0;
        command.Setup(x => x.ExecuteAsync())
            .ReturnsAsync(commandResult)
            .Callback(() => commandInvokeCount++);

        factory.Setup(x => x.Create(commandParameters.Object))
            .Returns(command.Object);

        var handler = new HTTPCommandHandler(
            logger,
            factory.Object,
            deserializer.Object);

        // Act
        var result = await handler.HandleAsync(request);

        // Assert
        result.Should().Be(commandResult);
        commandInvokeCount.Should().Be(1);
    }

    [Theory(DisplayName = $"The instance can't handle command if given bad request.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n\r ")]
    [InlineData("")]
    public async Task CanNotHandlerIfGivenBadRequestAsync(string request)
    {
        // Arrange
        var logger = Mock.Of<ILogger<HTTPCommandHandler>>();
        var factory = Mock.Of<ICommandFactory>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, CommandParametersBase>>(MockBehavior.Strict);

        var handler = new HTTPCommandHandler(
            logger,
            factory,
            deserializer);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            _ = await handler.HandleAsync(request));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
