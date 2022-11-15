using General.Storage;

using Import.Logic.Commands;
using Import.Logic.Models;

using Moq;

namespace Import.Logic.Tests.Commands;

public class SetLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        SetLinkCommand command = null!;
        var id = new CommandID("some id");
        var parameters = new SetLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var cache = Mock.Of<ICache<Link>>();
        var repository = Mock.Of<IRepository<Link>>();

        // Act
        var exception = Record.Exception(() =>
            command = new SetLinkCommand(parameters, cache, repository));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var cache = Mock.Of<ICache<Link>>();
        var repository = Mock.Of<IRepository<Link>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(parameters: null!, cache, repository));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new SetLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var repository = Mock.Of<IRepository<Link>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(parameters, cacheLinks: null!, repository));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutRepository()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new SetLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var cache = Mock.Of<ICache<Link>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new SetLinkCommand(parameters, cache, repository: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new SetLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var link = new Link(parameters.ExternalID, parameters.InternalID);

        var cache = new Mock<ICache<Link>>();
        var cacheInvokeCount = 0;
        cache.Setup(x => x.Contains(link))
            .Returns(false)
            .Callback(() => cacheInvokeCount++);

        var repository = new Mock<IRepository<Link>>();

        var command = new SetLinkCommand(
            parameters,
            cache.Object,
            repository.Object);

        var expectedResult = CommandResult.Success(id);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        cacheInvokeCount.Should().Be(1);

        cache.Verify(x => x.Add(link), Times.Once);

        repository.Verify(x => x.AddAsync(link, It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.Save(), Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can execute when link already exists.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteWhenLinkAlreadyExistsAsync()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new SetLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var link = new Link(parameters.ExternalID, parameters.InternalID);

        var repository = new Mock<IRepository<Link>>(MockBehavior.Strict);

        var cache = new Mock<ICache<Link>>();
        var cacheInvokeCount = 0;
        cache.Setup(x => x.Contains(link))
            .Returns(true)
            .Callback(() => cacheInvokeCount++);

        var command = new SetLinkCommand(
            parameters,
            cache.Object,
            repository.Object);

        var expectedResult = CommandResult.Fail(id, "some error message");

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result,
            opt => opt.Excluding(x => x.ErrorMessage));
        cacheInvokeCount.Should().Be(1);
    }
}
