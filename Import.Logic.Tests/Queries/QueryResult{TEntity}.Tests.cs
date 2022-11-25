using General.Logic.Executables;

using Import.Logic.Queries;

namespace Import.Logic.Tests.Queries;

public class QueryResultTests
{
    [Fact(DisplayName = $"The {nameof(QueryResult<FakeQueryResult>)} can create fail result.")]
    [Trait("Category", "Unit")]
    public void CanCreateFailResult()
    {
        // Arrange
        QueryResult<FakeQueryResult> commandResult = null!;
        var id = new ExecutableID("some id");
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeQueryResult>.Fail(id, errorMessage));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(errorMessage);
        commandResult.Result.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeQueryResult>)} can't create fail result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateFailResultWithoutId()
    {
        // Arrange
        QueryResult<FakeQueryResult> commandResult = null!;
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeQueryResult>.Fail(id: null!, errorMessage));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(QueryResult<FakeQueryResult>)} can't create fail result with bad error message.")]
    [Trait("Category", "Unit")]
    [InlineData("")]
    [InlineData(" \r\n \t ")]
    [InlineData(" ")]
    [InlineData(null!)]
    public void CanNotCreateFailResultWithBadErrorMessage(string message)
    {
        // Arrange
        QueryResult<FakeQueryResult> commandResult = null!;
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeQueryResult>.Fail(id, message));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeQueryResult>)} can create success result.")]
    [Trait("Category", "Unit")]
    public void CanCreateSuccessResult()
    {
        // Arrange
        QueryResult<FakeQueryResult> commandResult = null!;
        var entityResult = new FakeQueryResult();
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeQueryResult>.Success(id, entityResult));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(null);
        commandResult.Result.Should().Be(entityResult);
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeQueryResult>)} can't create success result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutId()
    {
        // Arrange
        QueryResult<FakeQueryResult> commandResult = null!;
        var entityResult = new FakeQueryResult();

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeQueryResult>.Success(id: null!, entityResult));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(QueryResult<FakeQueryResult>)} can't create success result without entity result.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutEntityResult()
    {
        // Arrange
        QueryResult<FakeQueryResult> commandResult = null!;
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = QueryResult<FakeQueryResult>.Success(id, result: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    private class FakeQueryResult
    {
    }
}
