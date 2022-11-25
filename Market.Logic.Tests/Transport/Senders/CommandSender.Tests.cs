using General.Logic.Executables;
using General.Transport;

using Market.Logic.Commands;
using Market.Logic.Transport.Senders;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

namespace Market.Logic.Tests.Transport.Senders;
public class CommandSenderTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<FakeCommand, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new FakeConfiguration("some destination"));

        // Act
        var exception = Record.Exception(() => _ = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options.Object,
            serializer));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var serializer = Mock.Of<ISerializer<FakeCommand, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new FakeConfiguration("some destination"));

        // Act
        var exception = Record.Exception(() => _ = new CommandSender<FakeConfiguration, FakeCommand>(
            logger: null!,
            options.Object,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without options.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutOptions()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<FakeCommand, string>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options: null!,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without serializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSerializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new FakeConfiguration("some destination"));

        // Act
        var exception = Record.Exception(() => _ = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options.Object,
            serializer: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without configuration.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutConfiguration()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<FakeCommand, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns((FakeConfiguration)null!);

        // Act
        var exception = Record.Exception(() => _ = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options.Object,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can send commands.")]
    [Trait("Category", "Unit")]
    public async Task CanSendCommandsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>();
        var serializer = new Mock<ISerializer<FakeCommand, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new FakeConfiguration("some destination"));

        var command = new FakeCommand(new("some id"));

        var serializerInvokeCount = 0;
        serializer.Setup(x => x.Serialize(command))
            .Returns("serialize message")
            .Callback(() => serializerInvokeCount++);

        var sender = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options.Object,
            serializer.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(command));

        // Assert
        exception.Should().BeNull();
        serializerInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The instance can't send when command is null.")]
    [Trait("Category", "Unit")]
    public async Task CanNotSendWithoutCommandAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>();
        var serializer = Mock.Of<ISerializer<FakeCommand, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new FakeConfiguration("some destination"));

        var sender = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options.Object,
            serializer);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can cancel opetation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelOperationAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CommandSender<FakeConfiguration, FakeCommand>>>();
        var serializer = Mock.Of<ISerializer<FakeCommand, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<FakeConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new FakeConfiguration("https://localhost:44376/api/courses"));

        var command = new FakeCommand(new("some id"));

        var cts = new CancellationTokenSource();

        var sender = new CommandSender<FakeConfiguration, FakeCommand>(
            logger,
            options.Object,
            serializer);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(command, cts.Token));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<OperationCanceledException>();
    }

    public sealed record class FakeConfiguration(string Destination) : ITransportSenderConfiguration;

    public sealed class FakeCommand : CommandBase
    {
        public FakeCommand(ExecutableID id) 
            : base(id, CommandType.SetLink)
        {
        }
    }
}
