using General.Logic.Commands;
using General.Logic.Executables;
using General.Storage;
using General.Transport;

using Moq;

using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport;

namespace WalletTransaction.Logic.Tests.Commands;

public class CancelTransactionRequestCommandTests
{
    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        CancelTransactionRequestCommand command = null!;
        
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");
        
        var parameters = new CancelTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            command = new CancelTransactionRequestCommand(parameters, cache));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(commandId);
    }

    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CancelTransactionRequestCommand(parameters: null!, cache));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommand)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new CancelTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new CancelTransactionRequestCommand(parameters, requestCache: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new CancelTransactionRequestCommandParameters(
            commandId,
            requestId);

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

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);

        cache.Setup(x => x.GetByKey(requestId))
            .Returns(request);

        var deleteInvokeCount = 0;
        cache.Setup(x => x.Delete(request))
            .Callback(() => deleteInvokeCount++);

        var command = new CancelTransactionRequestCommand(
            parameters,
            cache.Object);

        var expectedResult = CommandResult.Success(commandId);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        request.IsCancelled.Should().BeTrue();
        deleteInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CancelTransactionRequestCommand)} can execute when request is not exists.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteWhenRequestIsNotExistsAsync()
    {
        // Arrange
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new CancelTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);

        cache.Setup(x => x.GetByKey(requestId))
            .Returns((TransactionRequest)null!);

        var command = new CancelTransactionRequestCommand(
            parameters,
            cache.Object);

        var expectedResult = CommandResult.Fail(commandId, "some error message");

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result,
            opt => opt.Excluding(x => x.ErrorMessage));
    }
}
