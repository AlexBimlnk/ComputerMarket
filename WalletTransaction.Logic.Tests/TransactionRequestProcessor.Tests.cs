using General.Transport;

using Microsoft.Extensions.Logging;

using Moq;

using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic.Tests;
public class TransactionRequestProcessorTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionRequestProcessor>>(MockBehavior.Strict);
        var receiver = Mock.Of<IReceiver<ITransactionsRequest>>(MockBehavior.Strict);
        var resultSender = Mock.Of<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);
        var executer = Mock.Of<ITransactionRequestExecuter>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new TransactionRequestProcessor(
            logger,
            receiver,
            resultSender,
            executer));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var receiver = Mock.Of<IReceiver<ITransactionsRequest>>(MockBehavior.Strict);
        var resultSender = Mock.Of<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);
        var executer = Mock.Of<ITransactionRequestExecuter>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new TransactionRequestProcessor(
            logger: null!,
            receiver,
            resultSender,
            executer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without receiver.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutReceiver()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionRequestProcessor>>(MockBehavior.Strict);
        var resultSender = Mock.Of<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);
        var executer = Mock.Of<ITransactionRequestExecuter>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new TransactionRequestProcessor(
            logger,
            receiver: null!,
            resultSender,
            executer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without result sender.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutResultSender()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionRequestProcessor>>(MockBehavior.Strict);
        var receiver = Mock.Of<IReceiver<ITransactionsRequest>>(MockBehavior.Strict);
        var executer = Mock.Of<ITransactionRequestExecuter>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new TransactionRequestProcessor(
            logger,
            receiver,
            resultSender: null!,
            executer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without transactions executer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutExecuter()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionRequestProcessor>>(MockBehavior.Strict);
        var receiver = Mock.Of<IReceiver<ITransactionsRequest>>(MockBehavior.Strict);
        var resultSender = Mock.Of<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new TransactionRequestProcessor(
            logger,
            receiver,
            resultSender,
            transactionRequestExecuter: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can process and stop.")]
    [Trait("Category", "Unit")]
    public async Task CanProcessAndStopAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionRequestProcessor>>();
        var receiver = new Mock<IReceiver<ITransactionsRequest>>(MockBehavior.Strict);
        var resultSender = new Mock<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);
        var executer = new Mock<ITransactionRequestExecuter>(MockBehavior.Strict);

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.OldState)
            .Returns(TransactionRequestState.WaitHandle);
        request.Setup(x => x.IsCancelled)
            .Returns(false);

        var receiverCallbackCount = 0;
        receiver.Setup(x => x.ReceiveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(request.Object))
            .Callback(() => receiverCallbackCount++);

        var executerCallbackCount = 0;
        executer.Setup(x => x.ExecuteAsync(
                request.Object,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => executerCallbackCount++);

        var resultSenderCallbackCount = 0;
        resultSender.Setup(x => x.SendAsync(
                request.Object,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => resultSenderCallbackCount++);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        var processor = new TransactionRequestProcessor(
            logger,
            receiver.Object,
            resultSender.Object,
            executer.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await processor.ProcessAsync(cts.Token));

        // Assert
        exception.Should().NotBeNull()
            .And.BeOfType<OperationCanceledException>();
        receiverCallbackCount.Should().BeGreaterThan(0);
        executerCallbackCount.Should().BeGreaterThan(0);
        resultSenderCallbackCount.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = $"The instance can process and stop.")]
    [Trait("Category", "Unit")]
    public async Task CanNotProcessCancelledRequestAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TransactionRequestProcessor>>();
        var receiver = new Mock<IReceiver<ITransactionsRequest>>(MockBehavior.Strict);
        var resultSender = new Mock<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);
        var executer = new Mock<ITransactionRequestExecuter>(MockBehavior.Strict);

        var request = new Mock<ITransactionsRequest>(MockBehavior.Strict);
        request.SetupGet(x => x.OldState)
            .Returns(TransactionRequestState.WaitHandle);
        request.Setup(x => x.IsCancelled)
            .Returns(true);

        var receiverCallbackCount = 0;
        receiver.Setup(x => x.ReceiveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(request.Object))
            .Callback(() => receiverCallbackCount++);

        var executerCallbackCount = 0;
        executer.Setup(x => x.ExecuteAsync(
                request.Object,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => executerCallbackCount++);

        var resultSenderCallbackCount = 0;
        resultSender.Setup(x => x.SendAsync(
                request.Object,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => resultSenderCallbackCount++);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        var processor = new TransactionRequestProcessor(
            logger,
            receiver.Object,
            resultSender.Object,
            executer.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await processor.ProcessAsync(cts.Token));

        // Assert
        exception.Should().NotBeNull()
            .And.BeOfType<OperationCanceledException>();
        receiverCallbackCount.Should().BeGreaterThan(0);
        executerCallbackCount.Should().Be(0);
        resultSenderCallbackCount.Should().BeGreaterThan(0);
    }
}
