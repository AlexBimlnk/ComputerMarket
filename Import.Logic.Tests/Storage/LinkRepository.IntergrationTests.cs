using Import.Logic.Models;
using Import.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests.Storage;

using TLink = Logic.Storage.Models.Link;

public class LinkRepositoryIntegrationTests : DBIntegrationTestBase
{
    public LinkRepositoryIntegrationTests()
        : base(nameof(LinkRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} can add link.")]
    [Trait("Category", "Integration")]
    public async Task CanAddLinkAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Links)
            .Returns(_importContext.Links);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        var inputLink = new Link(new(2), new(4, Provider.HornsAndHooves));

        var expectedLink = new TLink[]
        {
            new TLink
            {
                InternalId = 2,
                ExternalId = 4,
                ProviderId = 2,
            }
        };

        var repository = new LinkRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _importContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputLink)
            .ConfigureAwait(false);

        await _importContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbLinks = await GetTableRecordsAsync(
            "links",
            r => new TLink
            {
                InternalId = r.GetInt64(0),
                ExternalId = r.GetInt64(1),
                ProviderId = r.GetInt16(2)
            });

        // Assert
        dbLinks.Should().BeEquivalentTo(expectedLink);
    }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} contains get true if link contains in database.")]
    [Trait("Category", "Integration")]
    public async Task ContainsGetTrueIfContainsLinkAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Links)
            .Returns(_importContext.Links);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        await AddLinkAsync(new(new(1), new(1, Provider.Ivanov)));
        await AddLinkAsync(new(new(2), new(4, Provider.Ivanov)));
        await AddLinkAsync(new(new(1), new(1, Provider.HornsAndHooves)));

        var inputLink = new Link(new(1), new(1, Provider.HornsAndHooves));

        var repository = new LinkRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputLink)
            .ConfigureAwait(false);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} can delete link.")]
    [Trait("Category", "Integration")]
    public void CanDeleteLink()
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

        var links = new Mock<DbSet<TLink>>(MockBehavior.Loose);

        context.Setup(x => x.Links)
            .Returns(links.Object);

        var linkRepository = new LinkRepository(
            context.Object,
            logger);

        var containsLink = new Link(
            new(1),
            new(1, Provider.Ivanov));

        // Act
        var exception = Record.Exception(() =>
            linkRepository.Delete(containsLink));

        // Assert
        exception.Should().BeNull();

        links.Verify(x =>
            x.Remove(
                It.Is<TLink>(l =>
                    l.InternalId == storageLink.InternalId &&
                    l.ExternalId == storageLink.ExternalId &&
                    l.ProviderId == storageLink.ProviderId &&
                    l.Provider == storageLink.Provider)),
            Times.Once);
    }

    private async Task AddLinkAsync(Link link)
    {
        var fromQuery = "links (internal_id, external_id, provider_id)";
        var valuesQuery = $"({link.InternalID.Value}, {link.ExternalID.Value}, {(short)link.ExternalID.Provider})";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddProviderAsync(Provider provider)
    {
        var fromQuery = "providers (id, provider_name)";
        var valuesQuery = $"({(short)provider}, '{provider}')";

        await AddAsync(fromQuery, valuesQuery);
    }
}
