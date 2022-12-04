using General.Logic.Executables;
using General.Storage;
using General.Transport;

using Moq;

using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic.Tests.Commands;

public class FinishTransactionRequestCommandTests
{
    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        FinishTransactionRequestCommand command = null!;
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");
        var parameters = new FinishTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            command = new FinishTransactionRequestCommand(parameters, sender, cache));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(commandId);
    }

    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new FinishTransactionRequestCommand(parameters: null!, sender, cache));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommand)} can't create without sender.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSender()
    {
        // Arrange
        var requestId = new InternalID(1);

        var commandId = new ExecutableID("some id");
        var parameters = new FinishTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new FinishTransactionRequestCommand(parameters, sender: null!, cache));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommand)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var requestId = new InternalID(1);

        var commandId = new ExecutableID("some id");
        var parameters = new FinishTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new FinishTransactionRequestCommand(parameters, sender, requestCache: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var requestId = new InternalID(1);

        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;
        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance)
        };

        var request = new TransactionRequest(requestId, transactions);

        var commandId = new ExecutableID("some id");
        var parameters = new FinishTransactionRequestCommandParameters(
            commandId,
            requestId);

        var sender = new Mock<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);

        cache.Setup(x => x.GetByKey(requestId))
            .Returns(request);

        var deleteInvokeCount = 0;
        cache.Setup(x => x.Delete(request))
            .Callback(() => deleteInvokeCount++);

        var command = new FinishTransactionRequestCommand(
            parameters,
            sender.Object,
            cache.Object);

        var expectedResult = CommandResult.Success(commandId);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        deleteInvokeCount.Should().Be(1);
        sender.Verify(x =>
            x.SendAsync(
                It.Is<ITransactionsRequest>(x =>
                    x.Id == request.Id &&
                    x.Transactions.Count == request.Transactions.Count &&
                    x.IsCancelled == request.IsCancelled),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(FinishTransactionRequestCommand)} can execute when request is not exists.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteWhenRequestIsNotExistsAsync()
    {
        // Arrange
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new FinishTransactionRequestCommandParameters(
            commandId,
            requestId);

        var sender = new Mock<ISender<TransactionSenderConfiguration, ITransactionsRequest>>(MockBehavior.Strict);

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);

        cache.Setup(x => x.GetByKey(requestId))
            .Returns((TransactionRequest)null!);

        var command = new FinishTransactionRequestCommand(
            parameters,
            sender.Object,
            cache.Object);

        var expectedResult = CommandResult.Fail(commandId, "some error message");

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result,
            opt => opt.Excluding(x => x.ErrorMessage));
    }
}
