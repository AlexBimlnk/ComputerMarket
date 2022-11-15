using General.Logic.Commands;
using General.Transport;

using Moq;

using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport;

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

        var cache = Mock.Of<ITransactionRequestCache>();
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
        var cache = Mock.Of<ITransactionRequestCache>();
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

        var cache = Mock.Of<ITransactionRequestCache>();

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

        var cache = Mock.Of<ITransactionRequestCache>();
        var sender = Mock.Of<ISender<TransactionSenderConfiguration, ITransactionsRequest>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CreateTransactionRequestCommand(parameters, sender, requestCache: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CreateTransactionRequestCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
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

        var cache = new Mock<ITransactionRequestCache>();
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
        cache.Verify(x => x.Add(parameters.TransactionRequest), Times.Once);
        sender.Verify(x =>
            x.SendAsync(
                parameters.TransactionRequest,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
