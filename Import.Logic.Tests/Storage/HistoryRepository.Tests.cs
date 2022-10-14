using Import.Logic.Models;
using Import.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests.Storage;

using THistory = Logic.Storage.Models.History;

public class HistoryRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new HistoryRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new HistoryRepository(
            context: null!,
            Mock.Of<ILogger>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new HistoryRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can add history.")]
    [Trait("Category", "Unit")]
    public async void CanAddHistoryAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var storageHistory = new THistory()
        {
            ExternalId = 1,
            ProviderId = 1,
            ProductMetadata = "product meta data"
        };

        var Histories = new Mock<DbSet<THistory>>(MockBehavior.Strict);
        var HistoriesCallback = 0;
        Histories.Setup(x => x
            .AddAsync(
                It.Is<THistory>(l =>
                    l.ExternalId == storageHistory.ExternalId &&
                    l.ProviderId == storageHistory.ProviderId &&
                    l.ProductMetadata == storageHistory.ProductMetadata),
                It.IsAny<CancellationToken>()))
            .Callback(() => HistoriesCallback++)
            .Returns(new ValueTask<EntityEntry<THistory>>());

        context.Setup(x => x.Histories)
            .Returns(Histories.Object);

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        var inputHistory = new History(
            new(1, Provider.Ivanov),
            productMetadata: "product meta data");

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await HistoryRepository.AddAsync(inputHistory));

        // Assert
        exception.Should().BeNull();
        HistoriesCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot add history when history is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddHistoryWhenHistoryIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await HistoryRepository.AddAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can cancel add history.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddHistoryAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var cts = new CancellationTokenSource();

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        var inputHistory = new History(
            new(1, Provider.Ivanov), 
            "product meta data");

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await HistoryRepository.AddAsync(inputHistory, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot contains history when history is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsWhenHistoryIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await HistoryRepository.ContainsAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can cancel contains history.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var cts = new CancellationTokenSource();

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        var inputHistory = new History(
            new(1, Provider.Ivanov),
            "product meta data");

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await HistoryRepository.ContainsAsync(inputHistory, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can delete history.")]
    [Trait("Category", "Unit")]
    public void CanDeleteHistory()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var storageHistory = new THistory()
        {
            ExternalId = 1,
            ProviderId = 1,
            ProductMetadata = "product meta data"
        };

        var Histories = new Mock<DbSet<THistory>>(MockBehavior.Loose);

        context.Setup(x => x.Histories)
            .Returns(Histories.Object);

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        var containsHistory = new History(
            new(1, Provider.Ivanov), 
            productMetadata: "product meta data");

        // Act
        var exception = Record.Exception(() =>
            HistoryRepository.Delete(containsHistory));

        // Assert
        exception.Should().BeNull();

        Histories.Verify(x =>
            x.Remove(
                It.Is<THistory>(l =>
                    l.ExternalId == storageHistory.ExternalId &&
                    l.ProviderId == storageHistory.ProviderId &&
                    l.ProductMetadata == storageHistory.ProductMetadata)),
            Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot delete History when History is null.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenHistoryIsNull()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger>();

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            HistoryRepository.Delete(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger>();

        var storageHistory = new THistory()
        {
            ExternalId = 1,
            ProviderId = 1,
            ProductMetadata = "product meta data"
        };

        var HistoryRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            HistoryRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
    }
}

