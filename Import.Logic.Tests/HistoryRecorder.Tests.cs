using Import.Logic.Abstractions;
using Import.Logic.Models;

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
        var repository = Mock.Of<IRepository<History>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HistoryRecorder(logger, repository));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var repository = Mock.Of<IRepository<History>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new HistoryRecorder(null!, repository));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutDeserializer()
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
    public async void CanRecordAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>();
        var repository = new Mock<IRepository<History>>();

        repository.Setup(x => x.AddAsync(It.IsAny<History>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var histories = new[]
        {
            new History(new(1, Provider.Ivanov), "some name", "description"),
            new History(new(1, Provider.HornsAndHooves), "some name")
        };

        var recorder = new HistoryRecorder(logger, repository.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await recorder.RecordHistoryAsync(histories));

        // Assert
        exception.Should().BeNull();
        repository.Verify(x => x.Save(), Times.Once);
    }

    [Fact(DisplayName = $"The instance can't record without history async.")]
    [Trait("Category", "Unit")]
    public async void CanNotRecordWithoutHistoryAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>();
        var repository = new Mock<IRepository<History>>();

        var recorder = new HistoryRecorder(logger, repository.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await recorder.RecordHistoryAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can cancel recordasync.")]
    [Trait("Category", "Unit")]
    public async void CanCancelRecordAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<HistoryRecorder>>();
        var repository = new Mock<IRepository<History>>();

        var histories = new[]
        {
            new History(new(1, Provider.Ivanov), "some name", "description"),
            new History(new(1, Provider.HornsAndHooves), "some name")
        };

        var cts = new CancellationTokenSource();

        var recorder = new HistoryRecorder(logger, repository.Object);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await recorder.RecordHistoryAsync(histories, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }
}
