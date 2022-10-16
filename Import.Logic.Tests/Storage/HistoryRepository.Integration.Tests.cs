using Import.Logic.Models;
using Import.Logic.Storage.Repositories;

using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests.Storage;

using THistory = Logic.Storage.Models.History;

public class HistoryRepositoryIntegrationTests : DBIntegrationTestBase
{
    public HistoryRepositoryIntegrationTests()
        : base(nameof(HistoryRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can add history.")]
    [Trait("Category", "Integration")]
    public async Task CanAddHistoryAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Histories)
            .Returns(_importContext.Histories);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        var inputHistory = new History(
            new(4, Provider.HornsAndHooves),
            productMetadata: "product_meta_data");

        var expectedHistory = new THistory[]
        {
            new THistory
            {
                ExternalId = 4,
                ProviderId = 2,
                ProductMetadata = "product_meta_data"
            }
        };

        var repository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _importContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputHistory)
            .ConfigureAwait(false);

        await _importContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbHistories = await GetTableRecordsAsync(
            "Histories",
            r => new THistory
            {
                ExternalId = (int)r.GetInt64(0),
                ProviderId = r.GetInt16(1),
                ProductMetadata = r.GetString(2)
            });

        // Assert
        dbHistories.Should().BeEquivalentTo(expectedHistory);
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can add history if already exists.")]
    [Trait("Category", "Integration")]
    public async Task CanAddHistoryIfAlreadyExistsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Histories)
            .Returns(_importContext.Histories);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        await AddHistoryAsync(new(new(1, Provider.Ivanov), "product1_meta_data"));

        var inputHistory = new History(
            new(1, Provider.Ivanov),
            productMetadata: "product_meta_data 2");

        var expectedHistory = new THistory[]
        {
            new THistory
            {
                ExternalId = 1,
                ProviderId = 1,
                ProductMetadata = "product_meta_data 2"
            }
        };

        var repository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _importContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputHistory)
            .ConfigureAwait(false);

        await _importContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbHistories = await GetTableRecordsAsync(
            "Histories",
            r => new THistory
            {
                ExternalId = (int)r.GetInt64(0),
                ProviderId = r.GetInt16(1),
                ProductMetadata = r.GetString(2)
            });

        // Assert
        dbHistories.Should().BeEquivalentTo(expectedHistory);
    }

    [Theory(DisplayName = $"The {nameof(HistoryRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(History inputHistory, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Histories)
            .Returns(_importContext.Histories);

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        await AddHistoryAsync(new(new(1, Provider.Ivanov), "product1_meta_data"));
        await AddHistoryAsync(new(new(4, Provider.Ivanov), "product2_meta_data"));
        await AddHistoryAsync(new(new(1, Provider.HornsAndHooves), "product3_meta_data"));

        var repository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputHistory)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can delete History.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteHistoryAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Histories)
            .Returns(_importContext.Histories);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _importContext.SaveChanges());

        await AddProviderAsync(Provider.Ivanov);
        await AddProviderAsync(Provider.HornsAndHooves);

        await AddHistoryAsync(new(new(1, Provider.Ivanov), "product1_meta_data"));
        await AddHistoryAsync(new(new(4, Provider.Ivanov), "product2_meta_data"));
        await AddHistoryAsync(new(new(1, Provider.HornsAndHooves), "product3_meta_data"));

        var repository = new HistoryRepository(
            context.Object,
            logger);

        var inputHistory = new History(
            new(1, Provider.Ivanov),
            productMetadata: "product1_meta_data");

        // Act
        var beforeContains = await repository.ContainsAsync(inputHistory)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputHistory);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputHistory)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
    }

    private async Task AddHistoryAsync(History history)
    {
        var fromQuery = "histories (external_id, provider_id, product_metadata)";
        var valuesQuery = $"({history.ExternalId.Value}, {(short)history.ExternalId.Provider}, \'{history.ProductMetadata}\')";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddProviderAsync(Provider provider)
    {
        var fromQuery = "providers (id, provider_name)";
        var valuesQuery = $"({(short)provider}, '{provider}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    public static readonly TheoryData<History, bool> ContainsData = new()
    {
        {
            new(new(1, Provider.Ivanov), "product1_meta_data"),
            true
        },
        {
            new(new(4, Provider.HornsAndHooves), "product2_meta_data"),
            false
        },
    };
}

