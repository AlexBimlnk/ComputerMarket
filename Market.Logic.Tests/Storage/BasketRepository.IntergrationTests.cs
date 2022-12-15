using System.Collections.Immutable;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TBasketItem = Logic.Storage.Models.BasketItem;
using TOrderItem = Logic.Storage.Models.OrderItem;

public class BasketRepositoryIntegrationTests : DBIntegrationTestBase
{
    public BasketRepositoryIntegrationTests()
        : base(nameof(BasketRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} can add product.")]
    [Trait("Category", "Integration")]
    public async Task CanAddProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.BasketItems)
            .Returns(_marketContext.BasketItems);

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        var expectedItems = new TBasketItem[]
        {
            TestHelper.GetStorageBasketItem(user, TestHelper.GetOrdinaryPurchasableEntity(product, 1))
        }.ToArray();

        var repository = new BasketRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddToBasketAsync(user, product)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbBasket = await GetTableRecordsAsync(
            "basket_items",
            r => new TBasketItem
            {
                UserId = r.GetInt64(0),
                ProviderId = r.GetInt64(1),
                ItemId = r.GetInt64(2),
                Quantity = r.GetInt32(3),
                User = null!,
                Product = null!
            });

        // Assert
        dbBasket.Should().BeEquivalentTo(
            expectedItems,
            options => options
            .Excluding(su => su.User)
            .Excluding(su => su.Product));
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} can delete product.")]
    [Trait("Category", "Integration")]
    public async Task CanRemoveProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.BasketItems)
            .Returns(_marketContext.BasketItems);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        await AddBasketItemAsync(user, product, 2);

        var repository = new BasketRepository(
            context.Object,
            logger);

        var inputOrder = TestHelper.GetOrdinaryOrder(user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        });

        // Act
        var exception = Record.Exception(() =>
        {
            repository.DeleteFromBasket(user, product);
            repository.Save();
        });

        var items = repository.GetAllBasketItems(user);

        // Assert
        exception.Should().BeNull();
        items.Any().Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} can get all basket items.")]
    [Trait("Category", "Integration")]
    public async Task CanGetEntitiesAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.BasketItems)
            .Returns(_marketContext.BasketItems);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        await AddBasketItemAsync(user, product, 2);

        var repository = new BasketRepository(
            context.Object,
            logger);

        var expectedResult = new PurchasableEntity[]
        {
            new PurchasableEntity(product, 2)
        };
        // Acr
        var result = repository.GetAllBasketItems(user).ToList();

        // Assert
        expectedResult.Should().BeEquivalentTo(result, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} can call multiple operations.")]
    [Trait("Category", "Integration")]
    public async Task CanCallMultipleMethodsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.BasketItems)
            .Returns(_marketContext.BasketItems);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        await AddBasketItemAsync(user, product, 2);

        var repository = new BasketRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            var result = repository.GetAllBasketItems(user);
            await repository.AddToBasketAsync(user, TestHelper.GetOrdinaryProduct(item, provider));
            await repository.AddToBasketAsync(user, TestHelper.GetOrdinaryProduct(item, provider));
            await repository.AddToBasketAsync(user, TestHelper.GetOrdinaryProduct(item, provider));

            repository.Save();
            result = repository.GetAllBasketItems(user);
            repository.RemoveFromBasket(user, TestHelper.GetOrdinaryProduct(item, provider));
            repository.RemoveFromBasket(user, TestHelper.GetOrdinaryProduct(item, provider));

            repository.Save();

            result = repository.GetAllBasketItems(user);
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

    private async Task AddItemAsync(Item item)
    {
        var fromQuery = "items (id, name, type_id)";
        var valuesQuery = $"({item.Key.Value}, '{item.Name}', {item.Type.Id})";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddBasketItemAsync(User user, Product product, int quantity)
    {
        var fromQuery = "basket_items (user_id, provider_id, item_id, quantity)";
        var valuesQuery = $"({user.Key.Value}, {product.Provider.Key.Value}, {product.Item.Key.Value}, '{quantity}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddItemTypeAsync(ItemType type)
    {
        var fromQuery = "item_type (id, name)";
        var valuesQuery = $"({type.Id}, '{type.Name}')";

        await AddAsync(fromQuery, valuesQuery);
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

    private async Task AddUserAsync(User user)
    {
        var fromQuery = "users (id, login, password, email, user_type_id)";
        var valuesQuery =
            $"({user.Key.Value}, " +
            $"'{user.AuthenticationData.Login}', " +
            $"'{user.AuthenticationData.Password.Value}', " +
            $"'{user.AuthenticationData.Email}', " +
            $"{(short)user.Type})";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddUserTypeAsync(UserType type)
    {
        var fromQuery = "user_type (id, name)";
        var valuesQuery = $"({(short)type}, '{type}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    public static readonly TheoryData<Order, bool> ContainsData = new()
    {
        {
            TestHelper.GetOrdinaryOrder(
                1,
                user: TestHelper.GetOrdinaryUser(1, type: UserType.Customer),
                entities: new HashSet<PurchasableEntity>()
                {
                    new PurchasableEntity(TestHelper.GetOrdinaryProduct(), 1)
                }),
            true
        },
        {
           TestHelper.GetOrdinaryOrder(
                2,
                user: TestHelper.GetOrdinaryUser(1, type: UserType.Customer),
                entities: new HashSet<PurchasableEntity>()
                {
                    new PurchasableEntity(TestHelper.GetOrdinaryProduct(), 1)
                }),
            false
        },
    };
}
