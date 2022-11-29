using System;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.Extensions.Logging;

using Moq;

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

        var provider1 = TestHelper.GetOrdinaryProvider();


        var type1 = TestHelper.GetOrdinaryItemType();

        var item1 = TestHelper.GetOrdinaryItem(type: type1);

        await AddProviderAsync(provider1);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);

        var inputProduct = TestHelper.GetOrdinaryProduct(item1, provider1);

        var expectedProduct = new TProduct[]
        {
            TestHelper.GetStorageProduct(inputProduct)
        }
        .Select(x => { x.Provider = null!; x.Item = null!; return x; })
        .ToArray();

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

        var provider1 = TestHelper.GetOrdinaryProvider(1, info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
            inn: "1111111111", 
            acc:"11111111111111111111"));

        var provider2 = TestHelper.GetOrdinaryProvider(2, info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
            inn: "2111111112",
            acc: "21111111111111111112"));

        var type1 = TestHelper.GetOrdinaryItemType();

        var item1 = TestHelper.GetOrdinaryItem(1, type1, "Item1");

        var item2 = TestHelper.GetOrdinaryItem(2, type1, "Item2");

        await AddProviderAsync(provider1);
        await AddProviderAsync(provider2);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);
        await AddItemAsync(item2);

        await AddProductAsync(TestHelper.GetOrdinaryProduct(item1, provider1));
        await AddProductAsync(TestHelper.GetOrdinaryProduct(item2, provider1));
        await AddProductAsync(TestHelper.GetOrdinaryProduct(item2, provider2));

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

        var provider = TestHelper.GetOrdinaryProvider();

        var type1 = TestHelper.GetOrdinaryItemType();

        var item1 = TestHelper.GetOrdinaryItem(1, type1, "Name1");

        var item2 = TestHelper.GetOrdinaryItem(2, type1, "Name2");

        await AddProviderAsync(provider);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);
        await AddItemAsync(item2);

        await AddProductAsync(TestHelper.GetOrdinaryProduct(item1, provider));
        await AddProductAsync(TestHelper.GetOrdinaryProduct(item2, provider));

        var repository = new ProductsRepository(
            context.Object,
            logger);

        var inputProduct = TestHelper.GetOrdinaryProduct(item1, provider);

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
            $"({product.Item.Key.Value}, " +
            $"{product.ProviderCost.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"{product.Quantity}, " +
            $"{product.Provider.Key.Value})";

        await AddAsync(fromQuery, valuesQuery);
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

    private async Task AddItemAsync(Item item)
    {
        var fromQuery = "items (id, name, type_id)";
        var valuesQuery = $"({item.Key.Value}, '{item.Name}', {item.Type.Id})";

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
            TestHelper.GetOrdinaryProduct(
                TestHelper.GetOrdinaryItem(
                    1, 
                    TestHelper.GetOrdinaryItemType(), 
                    "Name1"), 
                TestHelper.GetOrdinaryProvider(
                    1, 
                    info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
                        inn: "1111111111",
                        acc:"11111111111111111111"))),
            true
        },
        {
            TestHelper.GetOrdinaryProduct(
                TestHelper.GetOrdinaryItem(
                    4,
                    TestHelper.GetOrdinaryItemType(),
                    "Name4"),
                TestHelper.GetOrdinaryProvider(
                    4,
                    info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
                        inn: "4111111114",
                        acc:"41111111111111111114"))),
            false
        },
    };
}
