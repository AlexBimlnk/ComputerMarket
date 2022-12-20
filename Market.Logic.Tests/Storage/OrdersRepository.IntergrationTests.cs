using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.CompilerServices;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TOrder = Logic.Storage.Models.Order;
using TOrderItem = Logic.Storage.Models.OrderItem;

public class OrdersRepositoryIntegrationTests : DBIntegrationTestBase
{
    public OrdersRepositoryIntegrationTests()
        : base(nameof(OrdersRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can add order.")]
    [Trait("Category", "Integration")]
    public async Task CanAddOrderAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);

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

        var inputOrder = TestHelper.GetOrdinaryOrder(user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        });

        var expectedOrder = new TOrder[]
        {
            TestHelper.GetStorageOrder(inputOrder)
        }.ToArray();

        var expecredItems = expectedOrder[0].Items;

        var repository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputOrder)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbOrders = await GetTableRecordsAsync(
            "orders",
            r => new TOrder
            {
                Id = r.GetInt64(0),
                UserId = r.GetInt64(1),
                Date = r.GetDateTime(3),
                StateId = r.GetInt32(2),
                User = null!,
                Items = null!
            });

        var dbDescription = await GetTableRecordsAsync(
            "order_fill",
            r => new TOrderItem
            {
                OrderId = r.GetInt64(0),
                ProviderId = r.GetInt64(1),
                ItemId = r.GetInt64(2),
                Quantity = r.GetInt32(3),
                PaidPrice = r.GetDecimal(4),
                Product = null!,
                Order = null!
            });

        // Assert
        dbOrders.Should().BeEquivalentTo(
            expectedOrder,
            options => options
            .Excluding(su => su.User)
            .Excluding(su => su.Items)
            .Excluding(su => su.Date));

        dbDescription.Should().BeEquivalentTo(
            expecredItems,
            options => options
                .Excluding(su => su.Product)
                .Excluding(su => su.Order));
    }

    [Theory(DisplayName = $"The {nameof(OrdersRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(Order inputOrder, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);

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

        var order = TestHelper.GetOrdinaryOrder(1, user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        });

        await AddOrderAsync(order);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputOrder)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can delete order.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteOrdersAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);

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

        var order = TestHelper.GetOrdinaryOrder(user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        });

        await AddOrderAsync(order);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        var inputOrder = TestHelper.GetOrdinaryOrder(user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        });

        // Act
        var beforeContains = await repository.ContainsAsync(inputOrder)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputOrder);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputOrder)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can update order.")]
    [Trait("Category", "Integration")]
    public async Task CanUdpateOrderAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);

        var date = DateTime.Now;

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        var order = TestHelper.GetOrdinaryOrder(1, user: user, date, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        }).WithState(OrderState.PaymentWait);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        await repository.AddAsync(order);
        repository.Save();
        
        var updatedOrder = order.WithState(OrderState.ProviderAnswerWait);

        // Act
        var exception = Record.Exception(() =>
        {
            repository.UpdateState(updatedOrder);
            repository.Save();
        });

        var newOrder = repository.GetByKey(updatedOrder.Key);

        // Assert
        exception.Should().BeNull();
        newOrder.Should().BeEquivalentTo(updatedOrder, opt => opt.Excluding(x => x.OrderDate));
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can get all orders.")]
    [Trait("Category", "Integration")]
    public async Task CanGetEntitiesAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);
        var date = DateTime.Now;

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        var order = TestHelper.GetOrdinaryOrder(1, user: user, date, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        }).WithState(OrderState.PaymentWait);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        await repository.AddAsync(order);
        repository.Save();

        var expectedResult = new Order[]
        {
            TestHelper.GetOrdinaryOrder(1, user: user, date, entities: new HashSet<PurchasableEntity>()
            {
                new PurchasableEntity(product, 1)
            }).WithState(OrderState.PaymentWait)
        };

        // Acr
        var result = repository.GetEntities().ToList();

        // Assert
        expectedResult.Should().BeEquivalentTo(result, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can provider aprove.")]
    [Trait("Category", "Integration")]
    public async Task CanProvidersAproveAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);
        
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);
        var date = DateTime.Now;

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        var order = TestHelper.GetOrdinaryOrder(1, user: user, date, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        }).WithState(OrderState.ProviderAnswerWait);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        await repository.AddAsync(order);
        repository.Save();

        var expectedResult = order.WithState(OrderState.ProductDeliveryWait);

        // Acr
        repository.ProviderArpove(order, provider, true);
        repository.Save();
        var result = repository.GetByKey(order.Key);

        // Assert
        expectedResult.Should().BeEquivalentTo(result, opt => opt.WithStrictOrdering().Excluding(x => x.OrderDate));
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can get all provider aprove orders.")]
    [Trait("Category", "Integration")]
    public async Task CanGetProviderAproveOrdersAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);
        
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var userType = UserType.Customer;

        var user = TestHelper.GetOrdinaryUser(type: userType);

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Item name", Array.Empty<ItemProperty>());

        var provider = TestHelper.GetOrdinaryProvider();

        var product = TestHelper.GetOrdinaryProduct(item, provider);
        var date = DateTime.Now;

        await AddUserTypeAsync(userType);
        await AddUserAsync(user);
        await AddItemTypeAsync(type);
        await AddItemAsync(item);
        await AddProviderAsync(provider);
        await AddProductAsync(product);

        var order = TestHelper.GetOrdinaryOrder(1, user: user, date, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        }).WithState(OrderState.ProviderAnswerWait);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        await repository.AddAsync(order);
        repository.Save();

        var expectedResult = new Order[]
        {
            TestHelper.GetOrdinaryOrder(1, user: user, date, entities: new HashSet<PurchasableEntity>()
            {
                new PurchasableEntity(product, 1)
            }).WithState(OrderState.ProviderAnswerWait)
        };

        // Acr
        var result = repository.GetAproveOrdersOnProvider(provider).ToList();

        // Assert
        expectedResult.Should().BeEquivalentTo(result, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can call multiple operations.")]
    [Trait("Category", "Integration")]
    public async Task CanCallMultipleMethodsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Orders)
            .Returns(_marketContext.Orders);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

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

        var order1 = TestHelper.GetOrdinaryOrder(1, user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        }).WithState(OrderState.PaymentWait);

        var order2 = TestHelper.GetOrdinaryOrder(2, user: user, entities: new HashSet<PurchasableEntity>()
        {
            new PurchasableEntity(product, 1)
        }).WithState(OrderState.PaymentWait);

        var repository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await repository.AddAsync(order1);
            repository.Save();
            _ = repository.GetEntities();
            _ = repository.GetByKey(order1.Key);
            _ = repository.GetByKey(order1.Key);
            repository.UpdateState(order1);

            repository.Save();

            await repository.AddAsync(order2);

            repository.Save();

            _ = await repository.ContainsAsync(order2);
            _ = repository.GetEntities();
            _ = repository.GetByKey(order2.Key);
            _ = repository.GetByKey(order2.Key);

            repository.Delete(order2);

            repository.Save();

            var result = repository.GetEntities();
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

    private async Task AddOrderAsync(Order order)
    {
        var fromQuery = "orders (id, user_id, state_id, date)";
        var valuesQuery = $"({order.Key.Value}, {order.Creator.Key.Value}, {(int)order.State}, '{order.OrderDate}')";

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
