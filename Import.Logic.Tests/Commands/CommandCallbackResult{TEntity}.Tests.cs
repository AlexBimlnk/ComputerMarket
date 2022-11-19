using General.Logic.Commands;

using Import.Logic.Commands;

namespace Import.Logic.Tests.Commands;
public class CommandCallbackResultTests
{
    [Fact(DisplayName = $"The {nameof(CommandCallbackResult<FakeCallbackEntity>)} can create fail result.")]
    [Trait("Category", "Unit")]
    public void CanCreateFailResult()
    {
        // Arrange
        CommandCallbackResult<FakeCallbackEntity> commandResult = null!;
        var id = new CommandID("some id");
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandCallbackResult<FakeCallbackEntity>.Fail(id, errorMessage));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(errorMessage);
        commandResult.IsSuccess.Should().Be(false);
        commandResult.Result.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(CommandCallbackResult<FakeCallbackEntity>)} can't create fail result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateFailResultWithoutId()
    {
        // Arrange
        CommandCallbackResult<FakeCallbackEntity> commandResult = null!;
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandCallbackResult<FakeCallbackEntity>.Fail(id: null!, errorMessage));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(CommandCallbackResult<FakeCallbackEntity>)} can't create fail result with bad error message.")]
    [Trait("Category", "Unit")]
    [InlineData("")]
    [InlineData(" \r\n \t ")]
    [InlineData(" ")]
    [InlineData(null!)]
    public void CanNotCreateFailResultWithBadErrorMessage(string message)
    {
        // Arrange
        CommandCallbackResult<FakeCallbackEntity> commandResult = null!;
        var id = new CommandID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandCallbackResult<FakeCallbackEntity>.Fail(id, message));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(CommandCallbackResult<FakeCallbackEntity>)} can create success result.")]
    [Trait("Category", "Unit")]
    public void CanCreateSuccessResult()
    {
        // Arrange
        CommandCallbackResult<FakeCallbackEntity> commandResult = null!;
        var entityResult = new FakeCallbackEntity();
        var id = new CommandID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandCallbackResult<FakeCallbackEntity>.Success(id, entityResult));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(null);
        commandResult.IsSuccess.Should().Be(true);
        commandResult.Result.Should().Be(entityResult);
    }

    [Fact(DisplayName = $"The {nameof(CommandCallbackResult<FakeCallbackEntity>)} can't create success result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutId()
    {
        // Arrange
        CommandCallbackResult<FakeCallbackEntity> commandResult = null!;
        var entityResult = new FakeCallbackEntity();

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandCallbackResult<FakeCallbackEntity>.Success(id: null!, entityResult));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(CommandCallbackResult<FakeCallbackEntity>)} can't create success result without entity result.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutEntityResult()
    {
        // Arrange
        CommandCallbackResult<FakeCallbackEntity> commandResult = null!;
        var id = new CommandID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandCallbackResult<FakeCallbackEntity>.Success(id, result: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    private class FakeCallbackEntity
    {
    }
}
