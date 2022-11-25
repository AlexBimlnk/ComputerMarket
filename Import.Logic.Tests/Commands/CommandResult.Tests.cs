using General.Logic.Commands;
using General.Logic.Executables;

using Import.Logic.Commands;

namespace Import.Logic.Tests.Commands;
public class CommandResultTests
{
    [Fact(DisplayName = $"The {nameof(CommandResult)} can create fail result.")]
    [Trait("Category", "Unit")]
    public void CanCreateFailResult()
    {
        // Arrange
        CommandResult commandResult = null!;
        var id = new ExecutableID("some id");
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandResult.Fail(id, errorMessage));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(errorMessage);
        commandResult.IsSuccess.Should().Be(false);
    }

    [Fact(DisplayName = $"The {nameof(CommandResult)} can't create fail result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateFailResultWithoutId()
    {
        // Arrange
        CommandResult commandResult = null!;
        var errorMessage = "some error message";

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandResult.Fail(id: null!, errorMessage));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(CommandResult)} can't create fail result with bad error message.")]
    [Trait("Category", "Unit")]
    [InlineData("")]
    [InlineData(" \r\n \t ")]
    [InlineData(" ")]
    [InlineData(null!)]
    public void CanNotCreateFailResultWithBadErrorMessage(string message)
    {
        // Arrange
        CommandResult commandResult = null!;
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandResult.Fail(id, message));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(CommandResult)} can create success result.")]
    [Trait("Category", "Unit")]
    public void CanCreateSuccessResult()
    {
        // Arrange
        CommandResult commandResult = null!;
        var id = new ExecutableID("some id");

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandResult.Success(id));

        // Assert
        exception.Should().BeNull();
        commandResult.Id.Should().Be(id);
        commandResult.ErrorMessage.Should().Be(null);
        commandResult.IsSuccess.Should().Be(true);
    }

    [Fact(DisplayName = $"The {nameof(CommandResult)} can't create success result without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateSuccessResultWithoutId()
    {
        // Arrange
        CommandResult commandResult = null!;

        // Act
        var exception = Record.Exception(() =>
            commandResult = CommandResult.Success(id: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
