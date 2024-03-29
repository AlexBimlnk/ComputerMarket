﻿using General.Logic.Commands;
using General.Logic.Executables;
using General.Storage;

using Import.Logic.Commands;
using Import.Logic.Models;

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
        var id = new ExecutableID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1, Provider.Ivanov));
        var cache = Mock.Of<IKeyableCache<Link, ExternalID>>();
        var repository = Mock.Of<IRepository<Link>>();

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
        var cache = Mock.Of<IKeyableCache<Link, ExternalID>>();
        var repository = Mock.Of<IRepository<Link>>();

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
        var id = new ExecutableID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1, Provider.Ivanov));
        var repository = Mock.Of<IRepository<Link>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(parameters, cacheLinks: null!, repository));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutRepository()
    {
        // Arrange
        var id = new ExecutableID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1, Provider.Ivanov));
        var cache = Mock.Of<IKeyableCache<Link, ExternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new DeleteLinkCommand(parameters, cache, repository: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var link = new Link(new InternalID(1), new ExternalID(1, Provider.Ivanov));

        var id = new ExecutableID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1, Provider.Ivanov));

        var cache = new Mock<IKeyableCache<Link, ExternalID>>();
        var cacheInvokeCount = 0;
        cache.Setup(x => x.GetByKey(parameters.ExternalID))
            .Returns(link)
            .Callback(() => cacheInvokeCount++);

        var repository = new Mock<IRepository<Link>>();

        var command = new DeleteLinkCommand(
            parameters,
            cache.Object,
            repository.Object);

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

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't execute when link not exists.")]
    [Trait("Category", "Unit")]
    public async void CantExecuteWhenLinkNotExistsAsync()
    {
        // Arrange
        Link link = null!;

        var id = new ExecutableID("some id");
        var parameters = new DeleteLinkCommandParameters(
            id,
            new(1, Provider.Ivanov));

        var repository = new Mock<IRepository<Link>>(MockBehavior.Strict);

        var cache = new Mock<IKeyableCache<Link, ExternalID>>();
        var cacheInvokeCount = 0;
        cache.Setup(x => x.GetByKey(parameters.ExternalID))
            .Returns(link)
            .Callback(() => cacheInvokeCount++);

        var command = new DeleteLinkCommand(
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
