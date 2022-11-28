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

        var inputProvider = new Provider(
            new ID(1),
            "Provider Name 1",
            new Margin(1.3m),
            new PaymentTransactionsInformation(
                inn: "1234512345",
                bankAccount: "12345123451234512345"));

        var expectedProvider = new TProvider[]
        {
            new TProvider
            {
                Id = 1,
                Name = "Provider Name 1",
                Margin = 1.3m,
                Inn = "1234512345",
                BankAccount = "12345123451234512345"
            }
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
        var provider = new Provider(
            new ID(1),
            "Provider Name 1",
            new Margin(1.3m),
            new PaymentTransactionsInformation(
                inn: "1234512345",
                bankAccount: "12345123451234512345"));

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

        var provider1 = new Provider(
            id: new ID(1),
            "name1",
            new Margin(1.3m),
            new PaymentTransactionsInformation(
                inn: "1234512345",
                bankAccount: "12345123451234512345"));

        
        await AddProviderAsync(provider1);
        
        var repository = new ProvidersRepository(
            context.Object,
            logger);

        var inputProvider = new Provider(
            new ID(1),
            "Provider Name 1",
            new Margin(1.3m),
            new PaymentTransactionsInformation(
                inn: "1234512345",
                bankAccount: "12345123451234512345"));

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
            new Provider(
                new ID(1),
                "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    inn: "1234512345",
                    bankAccount: "12345123451234512345")),
            true
        },
        {
            new Provider(
                new ID(2),
                "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    inn: "1234512344",
                    bankAccount: "12345123451234512344")),
            false
        },
    };
}
