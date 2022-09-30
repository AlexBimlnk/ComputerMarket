using Import.Logic.Models;
using Import.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests.Storage;

using TLink = Logic.Storage.Models.Link;

public class LinkRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(LinkRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new LinkRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new LinkRepository(
            context: null!,
            Mock.Of<ILogger>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new LinkRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} can add link.")]
    [Trait("Category", "Unit")]
    public async void CanAddLinkAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var storageLink = new TLink()
        {
            InternalId = 1,
            ExternalId = 1,
            ProviderId = 1
        };

        var links = new Mock<DbSet<TLink>>(MockBehavior.Strict);
        var linksCallback = 0;
        links.Setup(x => x
            .AddAsync(
                It.Is<TLink>(l => 
                    l.InternalId == storageLink.InternalId &&
                    l.ExternalId == storageLink.ExternalId &&
                    l.ProviderId == storageLink.ProviderId &&
                    l.Provider == storageLink.Provider),
                It.IsAny<CancellationToken>()))
            .Callback(() => linksCallback++)
            .Returns(new ValueTask<EntityEntry<TLink>>());

        context.Setup(x => x.Links)
            .Returns(links.Object);

        var linkRepository = new LinkRepository(
            context.Object,
            logger);

        var inputLink = new Link(
            new(1),
            new(1, Provider.Ivanov));

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await linkRepository.AddAsync(inputLink));

        // Assert
        exception.Should().BeNull();
        linksCallback.Should().Be(1);
    }

    
}
