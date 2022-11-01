using System;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit.Abstractions;

namespace Market.Logic.Tests.Storage;

using TProduct = Logic.Storage.Models.Product;

public class ProductsRepositoryIntegrationTests : DBIntegrationTestBase
{
    public ProductsRepositoryIntegrationTests()
        : base(nameof(ProductsRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can add product.")]
    [Trait("Category", "Integration")]
    public async Task CanAddProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        var provider1 = new Provider(
                "name1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    inn: "1234512345",
                    bankAccount: "12345123451234512345"),
                id: 1);


        var type1 = new ItemType("Type1", id: 1);

        var item1 = new Item(
                type1,
                "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1);

        await AddProviderAsync(provider1);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);

        var inputProduct = new Product(item1, provider1, new Price(1000.30m), quantity: 3);

        var expectedProduct = new TProduct[]
        {
            new TProduct
            {
                ItemId = 1,
                ProviderId = 1,
                ProviderCost = 1000.30m,
                Quantity = 3,
            }
        };

        var repository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputProduct)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbProducts = await GetTableRecordsAsync(
            "products",
            r => new TProduct
            {
                ItemId = r.GetInt64(0),
                ProviderId = r.GetInt64(1),
                ProviderCost = r.GetDecimal(2),
                Quantity = r.GetInt32(3)
            });

        // Assert
        dbProducts.Should().BeEquivalentTo(expectedProduct);
    }

    [Theory(DisplayName = $"The {nameof(ProductsRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(Product inputProduct, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        var provider1 = new Provider(
                "name1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    inn: "1234512345",
                    bankAccount: "12345123451234512345"),
                id: 1);

        var provider2 = new Provider(
                "name2",
                new Margin(1.2m),
                new PaymentTransactionsInformation(
                    inn: "1234512344",
                    bankAccount: "12345123451234512344"),
                id: 2);

        var type1 = new ItemType("Type1", id: 1);

        var item1 = new Item(
                type1,
                "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1);

        var item2 = new Item(
                type1,
                "Name 2",
                Array.Empty<ItemProperty>(),
                id: 2);

        await AddProviderAsync(provider1);
        await AddProviderAsync(provider2);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);
        await AddItemAsync(item2);

        await AddProductAsync(new(item1, provider1, new Price(100.00m), quantity: 3));
        await AddProductAsync(new(item2, provider1, new Price(120.00m), quantity: 4));
        await AddProductAsync(new(item2, provider2, new Price(1000.00m), quantity: 5));
        
        var repository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputProduct)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can delete product.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var provider = new Provider(
                "name1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    inn: "1234512345",
                    bankAccount: "12345123451234512345"),
                id: 1);

        var type1 = new ItemType("Type1", id: 1);

        var item1 = new Item(
                type1,
                "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1);

        var item2 = new Item(
                type1,
                "Name 2",
                Array.Empty<ItemProperty>(),
                id: 2);

        await AddProviderAsync(provider);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);
        await AddItemAsync(item2);

        await AddProductAsync(new(item1, provider, new Price(100.00m), quantity: 3));
        await AddProductAsync(new(item2, provider, new Price(120.00m), quantity: 4));

        var repository = new ProductsRepository(
            context.Object,
            logger);

        var inputProduct = new Product(
            new Item(
                new ItemType("Type1", id: 1),
                "Name 2",
                Array.Empty<ItemProperty>(),
                id: 2),
            new Provider(
                "name1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    inn: "1234512345",
                    bankAccount: "12345123451234512345"),
                id: 1), 
            new Price(120.00m),
            quantity: 4);

        // Act
        var beforeContains = await repository.ContainsAsync(inputProduct)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputProduct);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputProduct)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
    }

    private async Task AddProductAsync(Product product)
    {
        var fromQuery = "products (item_id, provider_cost, quantity, provider_id)";
        var valuesQuery = 
            $"({product.Item.Id}, " +
            $"{product.ProviderCost.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"{product.Quantity}, " +
            $"{product.Provider.Id})";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddProviderAsync(Provider provider)
    {
        var fromQuery = "providers (id, name, margin, bank_account, inn)";
        var valuesQuery = 
            $"({provider.Id}, " +
            $"'{provider.Name}', " +
            $"{provider.Margin.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"'{provider.PaymentTransactionsInformation.BankAccount}', " +
            $"'{provider.PaymentTransactionsInformation.INN}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddItemAsync(Item item)
    {
        var fromQuery = "items (id, name, type_id)";
        var valuesQuery = $"({item.Id}, '{item.Name}', {item.Type.Id})";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddItemTypeAsync(ItemType type)
    {
        var fromQuery = "item_type (id, name)";
        var valuesQuery = $"({type.Id}, '{type.Name}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    public static readonly TheoryData<Product, bool> ContainsData = new()
    {
        {
            new(new Item(
                    new ItemType("Type1", id: 1),
                    "Name 2",
                    Array.Empty<ItemProperty>(),
                    id: 2), 
                new Provider(
                    "name1",
                    new Margin(1.3m),
                    new PaymentTransactionsInformation(
                        inn: "1234512345",
                        bankAccount: "12345123451234512345"),
                    id: 1), 
                new Price(120.00m), 
                quantity: 4),
            true
        },
        {
            new(new Item(
                    new ItemType("Type1", id: 1),
                    "Name 4",
                    Array.Empty<ItemProperty>(),
                    id: 4),
                new Provider(
                    "name4",
                    new Margin(1.3m),
                    new PaymentTransactionsInformation(
                        inn: "1234512345",
                        bankAccount: "12345123451234512345"),
                    id: 4),
                new Price(320.00m),
                quantity: 2),
            false
        },
    };
}
