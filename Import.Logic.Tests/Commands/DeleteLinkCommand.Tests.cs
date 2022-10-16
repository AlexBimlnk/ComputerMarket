using General.Storage;

using Import.Logic.Commands;
using Import.Logic.Models;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Import.Logic.Tests.Commands;

public class DeleteLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        DeleteLinkCommand command = null!;
        var id = new CommandID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var cache = Mock.Of<ICache<Link>>();
        var repository = Mock.Of<IServiceScopeFactory>();

        // Act
        var exception = Record.Exception(() =>
            command = new DeleteLinkCommand(parameters, cache, repository));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var cache = Mock.Of<ICache<Link>>();
        var repository = Mock.Of<IServiceScopeFactory>();

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(parameters: null!, cache, repository));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var scopeFactory = Mock.Of<IServiceScopeFactory>();

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(parameters, cacheLinks: null!, scopeFactory));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't create without scope factory.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutScopeFactory()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var cache = Mock.Of<ICache<Link>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(parameters, cache, scopeFactory: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var link = new Link(parameters.InternalID, parameters.ExternalID);

        var cache = new Mock<ICache<Link>>();
        var cacheInvokeCount = 0;
        cache.Setup(x => x.Contains(link))
            .Returns(true)
            .Callback(() => cacheInvokeCount++);

        var repository = new Mock<IRepository<Link>>();

        var scopeFactory = new Mock<IServiceScopeFactory>();
        var scope = new Mock<IServiceScope>();
        var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);

        scopeFactory.Setup(x => x.CreateScope())
            .Returns(scope.Object);

        scope.Setup(x => x.ServiceProvider)
            .Returns(serviceProvider.Object);

        serviceProvider.Setup(x => x.GetService(typeof(IRepository<Link>)))
            .Returns(repository.Object);

        var command = new DeleteLinkCommand(
            parameters,
            cache.Object,
            scopeFactory.Object);

        var expectedResult = CommandResult.Success(id);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        cacheInvokeCount.Should().Be(1);

        cache.Verify(x => x.Delete(link), Times.Once);

        repository.Verify(x => x.Delete(link), Times.Once);
        repository.Verify(x => x.Save(), Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can execute when link not exists.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteWhenLinkNotExistsAsync()
    {
        // Arrange
        var id = new CommandID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1),
            new(1, Provider.Ivanov));
        var link = new Link(parameters.InternalID, parameters.ExternalID);

        var cache = new Mock<ICache<Link>>();
        var cacheInvokeCount = 0;
        cache.Setup(x => x.Contains(link))
            .Returns(false)
            .Callback(() => cacheInvokeCount++);

        var repository = new Mock<IRepository<Link>>();

        var scopeFactory = new Mock<IServiceScopeFactory>();
        var scope = new Mock<IServiceScope>();
        var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);

        scopeFactory.Setup(x => x.CreateScope())
            .Returns(scope.Object);

        scope.Setup(x => x.ServiceProvider)
            .Returns(serviceProvider.Object);

        serviceProvider.Setup(x => x.GetService(typeof(IRepository<Link>)))
            .Returns(repository.Object);

        var command = new DeleteLinkCommand(
            parameters,
            cache.Object,
            scopeFactory.Object);

        var expectedResult = CommandResult.Fail(id, "some error message");

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result,
            opt => opt.Excluding(x => x.ErrorMessage));
        cacheInvokeCount.Should().Be(1);
    }
}
