using General.Logic.Commands;
using General.Logic.Executables;

using Moq;

using WalletTransaction.Logic.Commands;

namespace WalletTransaction.Logic.Tests.Commands;
public class CommandFactoryTests
{
    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var createRequest = Mock.Of<Func<CreateTransactionRequestCommandParameters, ICommand>>();
        var cancelRequest = Mock.Of<Func<CancelTransactionRequestCommandParameters, ICommand>>();
        var finishRequest = Mock.Of<Func<FinishTransactionRequestCommandParameters, ICommand>>();

        // Act
        var exception = Record.Exception(() => _ = new CommandFactory(
            createRequest, 
            cancelRequest,
            finishRequest));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create 'create request' command.")]
    [Trait("Category", "Unit")]
    public void CanCreateCreateRequestCommand()
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

        var createParameters = new CreateTransactionRequestCommandParameters(
            new ExecutableID("some id"),
            new TransactionRequest(id, transactions));

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var createRequest = new Mock<Func<CreateTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var cancelRequest = Mock.Of<Func<CancelTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var finishRequest = Mock.Of<Func<FinishTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);

        var createRequestInvokeCount = 0;
        createRequest.Setup(x => x.Invoke(createParameters))
            .Returns(command)
            .Callback(() => createRequestInvokeCount++);

        var factory = new CommandFactory(
            createRequest.Object, 
            cancelRequest,
            finishRequest);

        // Act
        ICommand result = null!;

        var exception = Record.Exception(() =>
            result = factory.Create(createParameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(command);
        createRequestInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create 'cancel request' command.")]
    [Trait("Category", "Unit")]
    public void CanCreateCancelRequestCommand()
    {
        // Arrange
        var cancelParameters = new CancelTransactionRequestCommandParameters(
            new("some id"),
            new(1));

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var createRequest = Mock.Of<Func<CreateTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var cancelRequest = new Mock<Func<CancelTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var finishRequest = Mock.Of<Func<FinishTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);

        var cancelRequestInvokeCount = 0;
        cancelRequest.Setup(x => x.Invoke(cancelParameters))
            .Returns(command)
            .Callback(() => cancelRequestInvokeCount++);

        var factory = new CommandFactory(
            createRequest, 
            cancelRequest.Object,
            finishRequest);

        // Act
        ICommand result = null!;

        var exception = Record.Exception(() =>
            result = factory.Create(cancelParameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(command);
        cancelRequestInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can create 'finish request' command.")]
    [Trait("Category", "Unit")]
    public void CanCreateFinishRequestCommand()
    {
        // Arrange
        var finishParameters = new FinishTransactionRequestCommandParameters(
            new("some id"),
            new(1));

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var createRequest = Mock.Of<Func<CreateTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var cancelRequest = Mock.Of<Func<CancelTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var finishRequest = new Mock<Func<FinishTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);

        var finishRequestInvokeCount = 0;
        finishRequest.Setup(x => x.Invoke(finishParameters))
            .Returns(command)
            .Callback(() => finishRequestInvokeCount++);

        var factory = new CommandFactory(
            createRequest,
            cancelRequest,
            finishRequest.Object);

        // Act
        ICommand result = null!;

        var exception = Record.Exception(() =>
            result = factory.Create(finishParameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(command);
        finishRequestInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(CommandFactory)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Arrange
        var createRequest = Mock.Of<Func<CreateTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var cancelRequest = Mock.Of<Func<CancelTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var finishRequest = Mock.Of<Func<FinishTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);

        var factory = new CommandFactory(
            createRequest, 
            cancelRequest,
            finishRequest);

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
        var createRequest = Mock.Of<Func<CreateTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var cancelRequest = Mock.Of<Func<CancelTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);
        var finishRequest = Mock.Of<Func<FinishTransactionRequestCommandParameters, ICommand>>(MockBehavior.Strict);

        var unknownParameters = new UnknownParameters(new("some id"));

        var factory = new CommandFactory(
            createRequest, 
            cancelRequest,
            finishRequest);

        // Act
        var exception = Record.Exception(() =>
            _ = factory.Create(unknownParameters));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    private class UnknownParameters : CommandParametersBase
    {
        public UnknownParameters(ExecutableID id) : base(id)
        {
        }
    }
}
