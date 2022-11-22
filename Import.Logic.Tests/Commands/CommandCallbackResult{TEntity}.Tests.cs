using General.Logic.Commands;
using General.Logic.Executables;

using Import.Logic.Queries;

namespace Import.Logic.Tests.Commands;
public class CommandCallbackResultTests
{
    [Fact(DisplayName = $"The {nameof(QueryResult<FakeCallbackEntity>)} can create fail result.")]
    [Trait("Category", "Unit")]
    public void CanCreateFailResult()
    {
        // Arrange
        QueryResult<FakeCallbackEntity> commandResult = null!;
        var id = new ExecutableID("some id");
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeCallbackEntity>.Fail(id, errorMessage));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(errorMessage);
        commandResult.Result.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeCallbackEntity>)} can't create fail result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateFailResultWithoutId()
    {
        // Arrange
        QueryResult<FakeCallbackEntity> commandResult = null!;
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeCallbackEntity>.Fail(id: null!, errorMessage));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(QueryResult<FakeCallbackEntity>)} can't create fail result with bad error message.")]
    [Trait("Category", "Unit")]
    [InlineData("")]
    [InlineData(" \r\n \t ")]
    [InlineData(" ")]
    [InlineData(null!)]
    public void CanNotCreateFailResultWithBadErrorMessage(string message)
    {
        // Arrange
        QueryResult<FakeCallbackEntity> commandResult = null!;
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeCallbackEntity>.Fail(id, message));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeCallbackEntity>)} can create success result.")]
    [Trait("Category", "Unit")]
    public void CanCreateSuccessResult()
    {
        // Arrange
        QueryResult<FakeCallbackEntity> commandResult = null!;
        var entityResult = new FakeCallbackEntity();
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeCallbackEntity>.Success(id, entityResult));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(null);
        commandResult.Result.Should().Be(entityResult);
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeCallbackEntity>)} can't create success result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutId()
    {
        // Arrange
        QueryResult<FakeCallbackEntity> commandResult = null!;
        var entityResult = new FakeCallbackEntity();

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeCallbackEntity>.Success(id: null!, entityResult));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeCallbackEntity>)} can't create success result without entity result.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutEntityResult()
    {
        // Arrange
        QueryResult<FakeCallbackEntity> commandResult = null!;
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeCallbackEntity>.Success(id, result: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    private class FakeCallbackEntity
    {
    }
}
