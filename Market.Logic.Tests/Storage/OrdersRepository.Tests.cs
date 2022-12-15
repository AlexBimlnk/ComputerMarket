using System.Net.WebSockets;

using General.Storage;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TOrder = Logic.Storage.Models.Order;

public class OrdersRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new OrdersRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger<OrdersRepository>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new OrdersRepository(
            context: null!,
            Mock.Of<ILogger<OrdersRepository>>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new OrdersRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} cannot add null order .")]
    [Trait("Category", "Unit")]
    public async void CanNotAddNullOrderAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var ordersRepository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await ordersRepository.AddAsync(entity: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can cancel add order.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddOrderAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var cts = new CancellationTokenSource();

        var orderRepository = new OrdersRepository(
            context.Object,
            logger);

        var inputOrder = TestHelper.GetOrdinaryOrder();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await orderRepository.AddAsync(inputOrder, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} cannot contains null order.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsNullOrderAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var orderRepository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await orderRepository.ContainsAsync(entity: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can cancel contains order.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var cts = new CancellationTokenSource();

        var ordersRepository = new OrdersRepository(
            context.Object,
            logger);

        var inputOrder = TestHelper.GetOrdinaryOrder();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await ordersRepository.ContainsAsync(inputOrder, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} cannot delete null order.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenNullOrder()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var ordersRepository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            ordersRepository.Delete(entity: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can get order by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var orderId = 1;
        var notExsistingOrderId = 2;
        var expectedResult = TestHelper.GetOrdinaryOrder(orderId);

        var data = new List<TOrder>
        {
            TestHelper.GetStorageOrder(expectedResult)
        }.AsQueryable();

        var orders = new Mock<DbSet<TOrder>>(MockBehavior.Strict);

        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Orders)
            .Returns(orders.Object);

        var ordersRepository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var result1 = ordersRepository.GetByKey(new ID(orderId));
        var result2 = ordersRepository.GetByKey(new ID(notExsistingOrderId));

        // Assert
        result1.Should().NotBeNull().And.BeEquivalentTo(expectedResult, opt => opt.Excluding(o => o.OrderDate));
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can get all orders.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var expectedResult = new List<Order>()
        {
            TestHelper.GetOrdinaryOrder()
        };

        var data = expectedResult
            .Select(x => TestHelper.GetStorageOrder(x))
            .AsQueryable();

        var orders = new Mock<DbSet<TOrder>>(MockBehavior.Strict);

        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        orders
            .As<IQueryable<TOrder>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Orders)
            .Returns(orders.Object);

        var ordersRepository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var result = ordersRepository.GetEntities();

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering().Excluding(o => o.OrderDate));
    }

    [Fact(DisplayName = $"The {nameof(OrdersRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<OrdersRepository>>();

        var ordersRepository = new OrdersRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            ordersRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
    }
}

