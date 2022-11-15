﻿using Import.Logic.Models;
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
        var logger = Mock.Of<ILogger<LinkRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Links)
            .Returns(_importContext.Links);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        var inputLink = new Link(new(4, Provider.HornsAndHooves), new(2));

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

    [Theory(DisplayName = $"The {nameof(LinkRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(Link inputLink, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<LinkRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Links)
            .Returns(_importContext.Links);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        await AddLinkAsync(new(new(1, Provider.Ivanov), new(1)));
        await AddLinkAsync(new(new(4, Provider.Ivanov), new(2)));
        await AddLinkAsync(new(new(1, Provider.HornsAndHooves), new(1)));

        var repository = new LinkRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputLink)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(LinkRepository)} can delete link.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteLinkAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<LinkRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Links)
            .Returns(_importContext.Links);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _importContext.SaveChanges());

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        await AddLinkAsync(new(new(1, Provider.Ivanov), new(1)));
        await AddLinkAsync(new(new(4, Provider.Ivanov), new(2)));
        await AddLinkAsync(new(new(1, Provider.HornsAndHooves), new(1)));

        var repository = new LinkRepository(
            context.Object,
            logger);

        var inputLink = new Link(new(1, Provider.Ivanov), new(1));

        // Act
        var beforeContains = await repository.ContainsAsync(inputLink)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputLink);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputLink)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
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

    public static readonly TheoryData<Link, bool> ContainsData = new()
    {
        {
            new(new(1, Provider.Ivanov), new(1)),
            true
        },
        {
            new(new(4, Provider.HornsAndHooves), new(2)),
            false
        },
    };
}
