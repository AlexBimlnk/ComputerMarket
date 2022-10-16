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
            Mock.Of<ILogger<HistoryRepository>>(MockBehavior.Strict)));

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
            Mock.Of<ILogger<HistoryRepository>>()));

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

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot add history when history is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddHistoryWhenHistoryIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await historyRepository.AddAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can cancel add history.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddHistoryAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var cts = new CancellationTokenSource();

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        var inputHistory = new History(
            new(1, Provider.Ivanov),
            "product meta data");

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await historyRepository.AddAsync(inputHistory, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} cannot contains history when history is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsWhenHistoryIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await historyRepository.ContainsAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can cancel contains history.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var cts = new CancellationTokenSource();

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        var inputHistory = new History(
            new(1, Provider.Ivanov),
            "product meta data");

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await historyRepository.ContainsAsync(inputHistory, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can delete history.")]
    [Trait("Category", "Unit")]
    public void CanDeleteHistory()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var storageHistory = new THistory()
        {
            ExternalId = 1,
            ProviderId = 1,
            ProductMetadata = "product meta data"
        };

        var histories = new Mock<DbSet<THistory>>(MockBehavior.Loose);

        context.Setup(x => x.Histories)
            .Returns(histories.Object);

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        var containsHistory = new History(
            new(1, Provider.Ivanov),
            productMetadata: "product meta data");

        // Act
        var exception = Record.Exception(() =>
            historyRepository.Delete(containsHistory));

        // Assert
        exception.Should().BeNull();

        histories.Verify(x =>
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
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            historyRepository.Delete(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(HistoryRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<HistoryRepository>>();

        var storageHistory = new THistory()
        {
            ExternalId = 1,
            ProviderId = 1,
            ProductMetadata = "product meta data"
        };

        var historyRepository = new HistoryRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            historyRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
    }
}

