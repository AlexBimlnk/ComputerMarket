using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TProvider = Logic.Storage.Models.Provider;

public class ProvidersRepositoryIntegrationTests : DBIntegrationTestBase
{
    public ProvidersRepositoryIntegrationTests()
        : base(nameof(ProvidersRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can add product.")]
    [Trait("Category", "Integration")]
    public async Task CanAddProviderAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Providers)
            .Returns(_marketContext.Providers);

        var inputProvider = TestHelper.GetOrdinaryProvider();

        var expectedProvider = new TProvider[]
        {
            TestHelper.GetStorageProvider(inputProvider)
        };

        var repository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputProvider)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbProviders = await GetTableRecordsAsync(
            "providers",
            r => new TProvider
            {
                Id = r.GetInt64(0),
                Name = r.GetString(1),
                Margin = r.GetDecimal(2),
                BankAccount = r.GetString(3),
                Inn = r.GetString(4)
            });

        // Assert
        dbProviders.Should().BeEquivalentTo(expectedProvider);
    }

    [Theory(DisplayName = $"The {nameof(ProvidersRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(Provider inputProvider, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Providers)
            .Returns(_marketContext.Providers);

        var provider = TestHelper.GetOrdinaryProvider();

        await AddProviderAsync(provider);
        
        var repository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputProvider)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can delete product.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteProviderAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Providers)
            .Returns(_marketContext.Providers);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var provider1 = TestHelper.GetOrdinaryProvider();

        await AddProviderAsync(provider1);
        
        var repository = new ProvidersRepository(
            context.Object,
            logger);

        var inputProvider = TestHelper.GetOrdinaryProvider();

        // Act
        var beforeContains = await repository.ContainsAsync(inputProvider)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputProvider);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputProvider)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can get all providers.")]
    [Trait("Category", "Integration")]
    public async Task CanGetEntitiesAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Providers)
            .Returns(_marketContext.Providers);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var provider1 = TestHelper.GetOrdinaryProvider();

        await AddProviderAsync(provider1);

        var repository = new ProvidersRepository(
            context.Object,
            logger);

        var expectedResult = new Provider[]
        {
            TestHelper.GetOrdinaryProvider()
        };

        var result = repository.GetEntities().ToList();

        // Assert
        expectedResult.Should().BeEquivalentTo(result, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can call multiple operations.")]
    [Trait("Category", "Integration")]
    public async Task CanCallMultipleMethodsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Providers)
            .Returns(_marketContext.Providers);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var provider1 = TestHelper.GetOrdinaryProvider(1);
        var provider2 = TestHelper.GetOrdinaryProvider(2, 
            info: TestHelper.GetOrdinaryPaymentTransactionsInformation("2222222222", "22222222222222222222"));
        
        await AddProviderAsync(provider1);

        var repository = new ProvidersRepository(
            context.Object,
            logger);


        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            _ = await repository.ContainsAsync(provider1);

            await repository.AddAsync(provider2);

            repository.Save();

            _ = repository.GetEntities();
            _ = repository.GetByKey(provider2.Key);
            _ = repository.GetByKey(provider2.Key);
        });

        // Assert
        exception.Should().BeNull();
    }

    private async Task AddProviderAsync(Provider provider)
    {
        var fromQuery = "providers (id, name, margin, bank_account, inn)";
        var valuesQuery =
            $"({provider.Key.Value}, " +
            $"'{provider.Name}', " +
            $"{provider.Margin.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"'{provider.PaymentTransactionsInformation.BankAccount}', " +
            $"'{provider.PaymentTransactionsInformation.INN}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    public static readonly TheoryData<Provider, bool> ContainsData = new()
    {
        {
            TestHelper.GetOrdinaryProvider(),
            true
        },
        {
            TestHelper.GetOrdinaryProvider(2, "Provider Name 2", TestHelper.GetOrdinaryPaymentTransactionsInformation("1111111111","11111111111111111111")),
            false
        },
    };
}
