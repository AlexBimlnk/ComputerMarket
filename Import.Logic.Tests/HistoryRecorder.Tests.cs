using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests;
public class HistoryRecorderTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>(MockBehavior.Strict);
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HistoryRecorder(logger, scopeFactory));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HistoryRecorder(null!, scopeFactory));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without scope factory.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutScopeFactory()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HistoryRecorder(logger, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can record history async.")]
    [Trait("Category", "Unit")]
    public async Task CanRecordAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>();
        var scopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict);

        var scope = new Mock<IServiceScope>();
        var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
        var repository = new Mock<IRepository<History>>();

        scopeFactory.Setup(x => x.CreateScope())
            .Returns(scope.Object);

        scope.Setup(x => x.ServiceProvider)
            .Returns(serviceProvider.Object);

        serviceProvider.Setup(x => x.GetService(typeof(IRepository<History>)))
            .Returns(repository.Object);

        var product = new Product(
            new ExternalID(1, Provider.Ivanov),
            new Price(100),
            7,
            "some metadata");

        var history = new History(new(1, Provider.Ivanov), "some metadata");

        var recorder = new HistoryRecorder(logger, scopeFactory.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await recorder.RecordHistoryAsync(product));

        // Assert
        exception.Should().BeNull();
        scope.Verify(x => x.Dispose(), Times.Once);
        repository.Verify(x => x.Save(), Times.Once);
        repository.Verify(x => 
            x.AddAsync(
                It.Is<History>(x => 
                    x.ExternalId == history.ExternalId && 
                    x.ProductMetadata == x.ProductMetadata), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact(DisplayName = $"The instance can't record without history async.")]
    [Trait("Category", "Unit")]
    public async Task CanNotRecordWithoutHistoryAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>();
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);

        var recorder = new HistoryRecorder(logger, scopeFactory);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await recorder.RecordHistoryAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can cancel recordasync.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelRecordAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>(MockBehavior.Strict);
        var scopeFactory = Mock.Of<IServiceScopeFactory>(MockBehavior.Strict);

        var product = new Product(
            new ExternalID(1, Provider.Ivanov),
            new Price(100),
            7,
            "some metadata");

        var cts = new CancellationTokenSource();

        var recorder = new HistoryRecorder(logger, scopeFactory);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await recorder.RecordHistoryAsync(product, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }
}
