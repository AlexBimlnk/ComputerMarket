using Import.Logic.Commands;

namespace Import.Logic.Tests.Commands;

public class SetLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        SetLinkCommand command = null!;
        var parameters = new SetLinkCommandParameters(new(1), new(1, Provider.Ivanov));
        var id = new CommandID("some id");

        // Act
        var exception = Record.Exception(() =>
            command = new SetLinkCommand(id, parameters));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutId()
    {
        // Arrange
        var parameters = new SetLinkCommandParameters(new(1), new(1, Provider.Ivanov));

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(id: null!, parameters));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var id = new CommandID("some id");

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(id, parameters: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var parameters = new SetLinkCommandParameters(new(1), new(1, Provider.Ivanov));
        var id = new CommandID("some id");
        var command = new SetLinkCommand(id, parameters);
        var expectedResult = CommandResult.Success(id);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
    }
}
