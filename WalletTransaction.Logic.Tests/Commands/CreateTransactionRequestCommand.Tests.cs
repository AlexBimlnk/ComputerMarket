using General.Logic.Commands;
using General.Storage;
using General.Transport;

using Moq;

using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic.Tests.Commands;

public class CreateTransactionRequestCommandTests
{
    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var id = new InternalID(1);

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

        var request = new TransactionRequest(id, transactions);

        CreateTransactionRequestCommand command = null!;
        var commandId = new CommandID("some id");
        var parameters = new CreateTransactionRequestCommandParameters(
            commandId,
            request);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            command = new CreateTransactionRequestCommand(parameters, sender, cache));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(commandId);
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CreateTransactionRequestCommand(parameters: null!, sender, cache));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can't create without sender.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSender()
    {
        // Arrange
        var id = new InternalID(1);

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

        var request = new TransactionRequest(id, transactions);

        var commandId = new CommandID("some id");
        var parameters = new CreateTransactionRequestCommandParameters(
            commandId,
            request);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CreateTransactionRequestCommand(parameters, sender: null!, cache));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var id = new InternalID(1);

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

        var request = new TransactionRequest(id, transactions);

        var commandId = new CommandID("some id");
        var parameters = new CreateTransactionRequestCommandParameters(
            commandId,
            request);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CreateTransactionRequestCommand(parameters, sender, requestCache: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} execute create proxy if request has other providers.")]
    [Trait("Category", "Unit")]
    public async void ExecuteCreateProxyIfRequestHasOtherProvidersAsync()
    {
        // Arrange
        var id = new InternalID(1);

        var fromAccount = new BankAccount("01234012340123401231");
        var toAccount = new BankAccount("01234012340123401232");
        var transferredBalance = 121.4m;
        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance)
        };

        var request = new TransactionRequest(id, transactions);

        var proxy = new ToMarketProxyTransactionRequest(request);

        var commandId = new CommandID("some id");
        var parameters = new CreateTransactionRequestCommandParameters(
            commandId,
            request);

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);
        var cacheAddInvokeCount = 0;
        cache.Setup(x => x.Add(request))
            .Callback(() => cacheAddInvokeCount++);

        var sender = new Mock<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        var command = new CreateTransactionRequestCommand(
            parameters,
            sender.Object,
            cache.Object);

        var expectedResult = CommandResult.Success(commandId);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        cacheAddInvokeCount.Should().Be(1);
        sender.Verify(x =>
            x.SendAsync(
                It.Is<ITransactionsRequest>(x =>
                    x.Id == proxy.Id &&
                    x.Transactions.Count == proxy.Transactions.Count &&
                    x.IsCancelled == proxy.IsCancelled),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} execute create default request if has only market provider.")]
    [Trait("Category", "Unit")]
    public async void ExecuteCreateDefaultRequestIfHasOnlyMarketProviderAsync()
    {
        // Arrange
        var id = new InternalID(1);

        var fromAccount = new BankAccount("01234012340123401231");
        var transferredBalance = 121.4m;
        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                ToMarketProxyTransactionRequest.MarketAccount,
                transferredBalance)
        };

        var request = new TransactionRequest(id, transactions);

        var commandId = new CommandID("some id");
        var parameters = new CreateTransactionRequestCommandParameters(
            commandId,
            request);

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);
        var cacheAddInvokeCount = 0;
        cache.Setup(x => x.Add(request))
            .Callback(() => cacheAddInvokeCount++);

        var sender = new Mock<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        var command = new CreateTransactionRequestCommand(
            parameters,
            sender.Object,
            cache.Object);

        var expectedResult = CommandResult.Success(commandId);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        cacheAddInvokeCount.Should().Be(1);
        sender.Verify(x =>
            x.SendAsync(
                request,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
