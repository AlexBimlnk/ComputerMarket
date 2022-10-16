using General.Transport;

using Import.Logic.Abstractions;
using Import.Logic.Models;
using Import.Logic.Transport.Senders;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

namespace Import.Logic.Tests.Transport.Senders;

using Configuration = Import.Logic.Transport.Configuration.InternalProductSenderConfiguration;

public class APIInternalProductSenderTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration());

        // Act
        var exception = Record.Exception(() => _ = new APIInternalProductSender(
            logger,
            options.Object,
            serializer));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var serializer = Mock.Of<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration());

        // Act
        var exception = Record.Exception(() => _ = new APIInternalProductSender(
            logger: null!,
            options.Object,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without options.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutOptions()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new APIInternalProductSender(
            logger,
            options: null!,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without serializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSerializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration());

        // Act
        var exception = Record.Exception(() => _ = new APIInternalProductSender(
            logger,
            options.Object,
            serializer: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without configuration.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutConfiguration()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>(MockBehavior.Strict);
        var serializer = Mock.Of<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns((Configuration)null!);

        // Act
        var exception = Record.Exception(() => _ = new APIInternalProductSender(
            logger,
            options.Object,
            serializer));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can send products.")]
    [Trait("Category", "Unit")]
    public async Task CanSendProductsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>();
        var serializer = new Mock<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var products = new List<Product>()
        {
            new(new(1, Provider.Ivanov), new(100), 1),
            new(new(1, Provider.HornsAndHooves), new(100), 2),
            new(new(2, Provider.Ivanov), new(100), 3),
        };

        var serializerInvokeCount = 0;
        serializer.Setup(x => x.Serialize(products))
            .Returns("serialize message")
            .Callback(() => serializerInvokeCount++);

        var sender = new APIInternalProductSender(
            logger,
            options.Object,
            serializer.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(products));

        // Assert
        exception.Should().BeNull();
        serializerInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The instance can send empty collection products.")]
    [Trait("Category", "Unit")]
    public async Task CanSendEmptyCollectionProductsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>();
        var serializer = new Mock<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var products = Enumerable.Empty<Product>()
            .ToList();

        var serializerInvokeCount = 0;
        serializer.Setup(x => x.Serialize(products))
            .Returns("serialize message")
            .Callback(() => serializerInvokeCount++);

        var sender = new APIInternalProductSender(
            logger,
            options.Object,
            serializer.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(products));

        // Assert
        exception.Should().BeNull();
        serializerInvokeCount.Should().Be(0);
    }

    [Fact(DisplayName = $"The instance can't send when collection is null.")]
    [Trait("Category", "Unit")]
    public async Task CanNotSendWithoutCollectionAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>();
        var serializer = Mock.Of<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var sender = new APIInternalProductSender(
            logger,
            options.Object,
            serializer);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can cancel opetation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelOperationAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIInternalProductSender>>();
        var serializer = Mock.Of<ISerializer<IReadOnlyCollection<Product>, string>>(MockBehavior.Strict);
        var options = new Mock<IOptions<Configuration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new Configuration() { Destination = "https://localhost:44376/api/courses" });

        var products = new List<Product>()
        {
            new(new(1, Provider.Ivanov), new(100), 1),
            new(new(1, Provider.HornsAndHooves), new(100), 2),
            new(new(2, Provider.Ivanov), new(100), 3),
        };

        var cts = new CancellationTokenSource();

        var sender = new APIInternalProductSender(
            logger,
            options.Object,
            serializer);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await sender.SendAsync(products, cts.Token));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<OperationCanceledException>();
    }
}
