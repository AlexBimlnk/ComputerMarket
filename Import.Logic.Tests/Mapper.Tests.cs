using General.Storage;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests;

public class MapperTests
{
    [Fact(DisplayName = $"The {nameof(Mapper)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<Link, ExternalID>>();
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = Mock.Of<IHistoryRecorder>();

        // Act
        var exception = Record.Exception(() => _ = new Mapper(
            cache,
            logger,
            historyRecorder));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<Link, ExternalID>>();
        var historyRecorder = Mock.Of<IHistoryRecorder>();

        // Act
        var exception = Record.Exception(() => _ = new Mapper(
            cache,
            logger: null!,
            historyRecorder));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = Mock.Of<IHistoryRecorder>();

        // Act
        var exception = Record.Exception(() => _ = new Mapper(
            cache: null!,
            logger,
            historyRecorder));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can map single entity.")]
    [Trait("Category", "Unit")]
    public async Task CanMapSingleProductAsync()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);
        var internalId = new InternalID(2);

        var product = new Product(externalId, new Price(1), 1);

        var link = new Link(externalId, internalId);

        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = Mock.Of<IHistoryRecorder>(MockBehavior.Strict);

        cache.Setup(x => x.GetByKey(externalId))
            .Returns(link);

        var mapper = new Mapper(
            cache.Object,
            logger,
            historyRecorder);

        // Act
        var result = await mapper.MapEntityAsync(product);

        // Assert
        result.InternalID.Should().BeEquivalentTo(internalId);
        result.ExternalID.Should().BeEquivalentTo(externalId);
        result.IsMapped.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} return back {nameof(Product)} when mapping without {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public async Task CanMapWhenEntityLinkIsNotInCacheAsync()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);

        var product = new Product(externalId, new Price(1), 1);

        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = new Mock<IHistoryRecorder>();

        cache.Setup(x => x.Contains(externalId))
            .Returns(false);

        var mapper = new Mapper(
            cache.Object,
            logger,
            historyRecorder.Object);

        // Act
        var result = await mapper.MapEntityAsync(product);

        // Assert
        result.ExternalID.Should().BeEquivalentTo(externalId);
        result.IsMapped.Should().BeFalse();
        historyRecorder.Verify(x =>
            x.RecordHistoryAsync(
                product,
                It.IsAny<CancellationToken>()),
            Times.Exactly(1));
    }

    [Fact(DisplayName = $"The instance can cancel single map operation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelMapSingleOperationAsync()
    {
        // Arrange
        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = new Mock<IHistoryRecorder>();

        var externalId = new ExternalID(1, Provider.Ivanov);
        var product = new Product(externalId, new Price(1), 1);

        var cts = new CancellationTokenSource();

        var mapper = new Mapper(
            cache.Object,
            logger,
            historyRecorder.Object);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            _ = await mapper.MapEntityAsync(product, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can map collection of entities.")]
    [Trait("Category", "Unit")]
    public async Task CanMapSuccesfullyWithLackOfLinksAsync()
    {
        // Arrange
        var links = new Link[2]
        {
            new Link(new ExternalID(1,Provider.Ivanov), new InternalID(1)),
            new Link(new ExternalID(2,Provider.HornsAndHooves), new InternalID(2))
        };

        var mappableProducts = new Product[2]
        {
            new Product(links[0].ExternalID, new Price(1), 3),
            new Product(links[1].ExternalID, new Price(2), 2)
        };

        var noneMappableProduct = new Product(
            new ExternalID(3, Provider.Ivanov),
            new Price(3),
            quantity: 1);

        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = new Mock<IHistoryRecorder>();

        cache.Setup(x => x.GetByKey(links[0].ExternalID))
            .Returns(links[0]);
        cache.Setup(x => x.GetByKey(links[1].ExternalID))
            .Returns(links[1]);

        var mapper = new Mapper(
            cache.Object,
            logger,
            historyRecorder.Object);

        // Act
        var result = await mapper.MapCollectionAsync(
            mappableProducts
                .Append(noneMappableProduct)
                .ToList());

        // Assert
        result.Select(x => x.IsMapped)
           .Should().AllBeEquivalentTo(true);

        result.Select(x => new { x.ExternalID, x.InternalID })
            .Should().BeEquivalentTo(
                mappableProducts.Select(x => new { x.ExternalID, x.InternalID }),
                opt => opt.WithStrictOrdering());

        noneMappableProduct.IsMapped.Should().BeFalse();

        historyRecorder.Verify(x =>
            x.RecordHistoryAsync(
                noneMappableProduct,
                It.IsAny<CancellationToken>()),
            Times.Exactly(1));
    }

    [Fact(DisplayName = $"The instance can cancel map collection operation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelMapCollectionOperationAsync()
    {
        // Arrange
        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<Mapper>>();
        var historyRecorder = new Mock<IHistoryRecorder>();

        var externalId = new ExternalID(1, Provider.Ivanov);
        var products = new[]
        {
            new Product(externalId, new Price(1), 1)
        };

        var cts = new CancellationTokenSource();

        var mapper = new Mapper(
            cache.Object,
            logger,
            historyRecorder.Object);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            _ = await mapper.MapCollectionAsync(products, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }
}