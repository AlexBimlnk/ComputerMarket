using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests;

using Configuration = Logic.Transport.Configuration.InternalProductSenderConfiguration;

public class APIExternalProductsHandlerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);
        var sender = Mock.Of<ISender<Configuration, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIExternalProductsHandler<FakeExternalProduct>(
                logger,
                fetcher,
                mapper,
                sender));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);
        var sender = Mock.Of<ISender<Configuration, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIExternalProductsHandler<FakeExternalProduct>(
                logger: null!,
                fetcher,
                mapper,
                sender));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without product fetcher.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProductFetcher()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);
        var sender = Mock.Of<ISender<Configuration, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIExternalProductsHandler<FakeExternalProduct>(
                logger,
                fetcher: null!,
                mapper,
                sender));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without mapper.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutMapper()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var sender = Mock.Of<ISender<Configuration, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIExternalProductsHandler<FakeExternalProduct>(
                logger,
                fetcher,
                mapper: null!,
                sender));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without history sender.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSender()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIExternalProductsHandler<FakeExternalProduct>(
                logger,
                fetcher,
                mapper,
                productsSender: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can fetch products.")]
    [Trait("Category", "Unit")]
    public async Task CanHandleProductsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>();
        var fetcher = new Mock<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = new Mock<IMapper<Product>>(MockBehavior.Strict);
        var sender = new Mock<ISender<Configuration, IReadOnlyCollection<Product>>>();

        var request = "some request";

        IReadOnlyCollection<Product> products = new Product[]
        {
            new(new(1, Provider.Ivanov), new(100), 2),
            new(new(2, Provider.Ivanov), new(100), 2),
            new(new(1, Provider.HornsAndHooves), new(100), 2)
        };

        var fetcherInvokeCount = 0;
        fetcher.Setup(x => x.FetchProductsAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products)
            .Callback(() => fetcherInvokeCount++);

        var mapperInvokeCount = 0;
        mapper.Setup(x => x.MapCollectionAsync(products, It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult(products))
            .Callback(() => mapperInvokeCount++);

        var handler = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            fetcher.Object,
            mapper.Object,
            sender.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await handler.HandleAsync(request));

        // Assert
        exception.Should().BeNull();
        fetcherInvokeCount.Should().Be(1);
        mapperInvokeCount.Should().Be(1);
        sender.Verify(x =>
            x.SendAsync(products, It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Theory(DisplayName = $"The instance can't fetch products if given bad request.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n\r ")]
    [InlineData("")]
    public async Task CanNotFetchProductsIfGivenBadRequestAsync(string request)
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);
        var sender = Mock.Of<ISender<Configuration, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var handler = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            fetcher,
            mapper,
            sender);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await handler.HandleAsync(request));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The instance can cancel operation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelOperationAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);
        var sender = Mock.Of<ISender<Configuration, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        var request = "some request";
        var cts = new CancellationTokenSource();

        var handler = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            fetcher,
            mapper,
            sender);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await handler.HandleAsync(request, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    public record class FakeExternalProduct();
}
