using General.Transport;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using WalletTransaction.Logic;
using WalletTransaction.Logic.Transport.Senders;

namespace Import.Logic.Tests.Transport.Senders;

using Configuration = WalletTransaction.Logic.Transport.Configurations.TransactionsResultSenderConfiguration;

public class TransactionsResultSenderSenderTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionsResultSender>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration());

        // Act
        var exception = Record.Exception(() => _ = new TransactionsResultSender(
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
        var serializer = Mock.Of<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration());

        // Act
        var exception = Record.Exception(() => _ = new TransactionsResultSender(
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
        var logger = Mock.Of<ILogger<TransactionsResultSender>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new TransactionsResultSender(
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
        var logger = Mock.Of<ILogger<TransactionsResultSender>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration());

        // Act
        var exception = Record.Exception(() => _ = new TransactionsResultSender(
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
        var logger = Mock.Of<ILogger<TransactionsResultSender>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns((Configuration)null!);

        // Act
        var exception = Record.Exception(() => _ = new TransactionsResultSender(
            logger,
            options.Object,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can send result request.")]
    [Trait("Category", "Unit")]
    public async Task CanSendResultRequestAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionsResultSender>>();
        var serializer = new Mock<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.Id)
            .Returns(It.IsAny<InternalID>());

        var serializerInvokeCount = 0;
        serializer.Setup(x => x.Serialize(request.Object))
            .Returns("serialize message")
            .Callback(() => serializerInvokeCount++);

        var sender = new TransactionsResultSender(
            logger,
            options.Object,
            serializer.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(request.Object));

        // Assert
        exception.Should().BeNull();
        serializerInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The instance can't send when result request is null.")]
    [Trait("Category", "Unit")]
    public async Task CanNotSendWithoutRequestAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionsResultSender>>();
        var serializer = Mock.Of<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var sender = new TransactionsResultSender(
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
        var logger = Mock.Of<ILogger<TransactionsResultSender>>();
        var serializer = Mock.Of<ISerializer<ITransactionsRequest, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var request = Mock.Of<ITransactionsRequest>(MockBehavior.Strict);

        using var cts = new CancellationTokenSource();

        var sender = new TransactionsResultSender(
            logger,
            options.Object,
            serializer);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(request, cts.Token));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<OperationCanceledException>();
    }
}
