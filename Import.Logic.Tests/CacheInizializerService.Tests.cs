using General.Storage;

using Import.Logic.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests;
public class CacheInizializerServiceTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CacheInizializerService>>(MockBehavior.Strict);
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);
        var cache = Mock.Of<ICache<Link>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new CacheInizializerService(logger, cache, scopeFactory));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);
        var cache = Mock.Of<ICache<Link>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new CacheInizializerService(
            logger: null!,
            cache,
            scopeFactory));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CacheInizializerService>>(MockBehavior.Strict);
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new CacheInizializerService(
            logger,
            cache: null!,
            scopeFactory));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without scope factory.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutScopeFactory()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CacheInizializerService>>(MockBehavior.Strict);
        var cache = Mock.Of<ICache<Link>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new CacheInizializerService(
            logger,
            cache,
            scopeFactory: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can start.")]
    [Trait("Category", "Unit")]
    public async Task CanStartAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CacheInizializerService>>();
        var scopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
        var cache = new Mock<ICache<Link>>(MockBehavior.Strict);

        var scope = new Mock<IServiceScope>();
        var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
        var repository = new Mock<IRepository<Link>>(MockBehavior.Strict);

        var links = new[]
        {
            new Link(new (1), new(2, Provider.Ivanov)),
            new Link(new (1), new(2, Provider.HornsAndHooves)),
        };

        var cacheAddRangeInvokeCount = 0;
        cache.Setup(x => x.AddRange(links.ToList()))
            .Callback(() => cacheAddRangeInvokeCount++);

        repository.Setup(x => x.GetEntities())
            .Returns(links.AsQueryable());

        scopeFactory.Setup(x => x.CreateScope())
            .Returns(scope.Object);

        scope.Setup(x => x.ServiceProvider)
            .Returns(serviceProvider.Object);

        serviceProvider.Setup(x => x.GetService(typeof(IRepository<Link>)))
            .Returns(repository.Object);

        var service = new CacheInizializerService(
            logger,
            cache.Object,
            scopeFactory.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await service.StartAsync());

        // Assert
        exception.Should().BeNull();
        cacheAddRangeInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The instance can cancel operation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelOperationAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CacheInizializerService>>(MockBehavior.Strict);
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);
        var cache = Mock.Of<ICache<Link>>(MockBehavior.Strict);

        var cts = new CancellationTokenSource();

        var service = new CacheInizializerService(
            logger,
            cache,
            scopeFactory);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await service.StartAsync(cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }
}
