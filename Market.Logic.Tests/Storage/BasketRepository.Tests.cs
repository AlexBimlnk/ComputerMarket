using System.Net.WebSockets;

using General.Storage;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TBasketItem = Logic.Storage.Models.BasketItem;

public class BasketRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(BasketRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new BasketRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger<BasketRepository>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new BasketRepository(
            context: null!,
            Mock.Of<ILogger<BasketRepository>>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new BasketRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} cannot add null order .")]
    [Trait("Category", "Unit")]
    public async void CanNotAddNullProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var ordersRepository = new BasketRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await ordersRepository.AddToBasketAsync(TestHelper.GetOrdinaryUser(), product: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} cannot add null order .")]
    [Trait("Category", "Unit")]
    public async void CanNotAddWhenUserIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var ordersRepository = new BasketRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await ordersRepository.AddToBasketAsync(user: null!, TestHelper.GetOrdinaryProduct()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} can cancel add order.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddOrderAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var cts = new CancellationTokenSource();

        var orderRepository = new BasketRepository(
            context.Object,
            logger);

        var inputProduct = TestHelper.GetOrdinaryProduct();
        var user = TestHelper.GetOrdinaryUser();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await orderRepository.AddToBasketAsync(user ,inputProduct, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} cannot delete null order.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenNullUser()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var ordersRepository = new BasketRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            ordersRepository.RemoveFromBasket(user: null!, TestHelper.GetOrdinaryProduct()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} cannot delete null order.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenNullProduct()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var ordersRepository = new BasketRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            ordersRepository.RemoveFromBasket(TestHelper.GetOrdinaryUser(), product: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(BasketRepository)} can get all orders.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var expectedResult = new List<PurchasableEntity>()
        {
            TestHelper.GetOrdinaryPurchasableEntity()
        };

        var user = TestHelper.GetOrdinaryUser();

        var data = expectedResult
            .Select(x => TestHelper.GetStorageBasketItem(user, x))
            .AsQueryable();

        var orders = new Mock<DbSet<TBasketItem>>(MockBehavior.Strict);

        orders
            .As<IQueryable<TBasketItem>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        orders
            .As<IQueryable<TBasketItem>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        orders
            .As<IQueryable<TBasketItem>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        orders
            .As<IQueryable<TBasketItem>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.BasketItems)
            .Returns(orders.Object);

        var ordersRepository = new BasketRepository(
            context.Object,
            logger);

        // Act
        var result = ordersRepository.GetAllBasketItems(user);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }


    [Fact(DisplayName = $"The {nameof(BasketRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<BasketRepository>>();

        var ordersRepository = new BasketRepository(
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

