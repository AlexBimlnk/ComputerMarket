using General.Logic.Executables;
using General.Storage;

using Moq;

using WalletTransaction.Logic.Commands;

namespace WalletTransaction.Logic.Tests.Commands;

public class RefundTransactionRequestCommandTests
{
    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        RefundTransactionRequestCommand command = null!;

        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new RefundTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            command = new RefundTransactionRequestCommand(parameters, cache));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(commandId);
    }

    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new RefundTransactionRequestCommand(parameters: null!, cache));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommand)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new RefundTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = Mock.Of<IKeyableCache<TransactionRequest, InternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new RefundTransactionRequestCommand(parameters, requestCache: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(RefundTransactionRequestCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var requestId = new InternalID(1);
        var commandId = new ExecutableID("some id");

        var parameters = new RefundTransactionRequestCommandParameters(
            commandId,
            requestId);

        var cache = new Mock<IKeyableCache<TransactionRequest, InternalID>>(MockBehavior.Strict);

        var command = new RefundTransactionRequestCommand(
            parameters,
            cache.Object);

        var expectedResult = CommandResult.Success(commandId);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
    }
}
