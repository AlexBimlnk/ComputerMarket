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

        // Act
        var exception = Record.Exception(() =>
            _ = new Mapper(cache, logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var cache = Mock.Of<IKeyableCache<Link, ExternalID>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new Mapper(cache, logger: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Mapper>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new Mapper(cache: null!, logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can map single entity.")]
    [Trait("Category", "Unit")]
    public void CanMapSingleProduct()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);
        var internalId = new InternalID(2);

        var product = new Product(externalId, new Price(1), 1);

        var link = new Link(internalId, externalId);

        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<Mapper>>();

        cache.Setup(x => x.GetByKey(externalId))
            .Returns(link);

        var mapper = new Mapper(cache.Object, logger);

        // Act
        var result = mapper.MapEntity(product);

        // Assert
        result.InternalID.Should().BeEquivalentTo(internalId);
        result.ExternalID.Should().BeEquivalentTo(externalId);
        result.IsMapped.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} return back {nameof(Product)} when mapping without {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanMapWhenEntityLinkIsNotInCache()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);

        var product = new Product(externalId, new Price(1), 1);

        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<Mapper>>();

        cache.Setup(x => x.Contains(externalId))
            .Returns(false);

        var mapper = new Mapper(cache.Object, logger);

        // Act
        var result = mapper.MapEntity(product);

        // Assert
        result.ExternalID.Should().BeEquivalentTo(externalId);
        result.IsMapped.Should().BeFalse();
    }


    [Fact(DisplayName = $"The {nameof(Mapper)} can map collection of entities.")]
    [Trait("Category", "Unit")]
    public void CanMapSuccesfullyWithLackOfLinks()
    {
        //Arrange
        var links = new Link[2]
        {
            new Link(new InternalID(1), new ExternalID(1,Provider.Ivanov)),
            new Link(new InternalID(2), new ExternalID(2,Provider.HornsAndHooves))
        };

        var mappableProducts = new Product[2]
        {
            new Product(links[0].ExternalID, new Price(1), 3),
            new Product(links[1].ExternalID, new Price(2), 2)
        };

        var noneMappableProducts = new Product[1]
        {
            new Product(new ExternalID(3,Provider.Ivanov), new Price(3), 1)
        };

        var cache = new Mock<IKeyableCache<Link, ExternalID>>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<Mapper>>();

        cache.Setup(x => x.GetByKey(links[0].ExternalID))
            .Returns(links[0]);
        cache.Setup(x => x.GetByKey(links[1].ExternalID))
            .Returns(links[1]);

        var mapper = new Mapper(cache.Object, logger);

        //Act
        var result = mapper.MapCollection(mappableProducts.Concat(noneMappableProducts).ToList());

        //Assert
        result.Select(x => x.IsMapped)
           .Should().AllBeEquivalentTo(true);

        result.Select(x => x.ExternalID)
            .Should().BeEquivalentTo(
                mappableProducts.Select(x => x.ExternalID),
                opt => opt.WithStrictOrdering());

        result.Select(x => x.InternalID)
            .Should().BeEquivalentTo(
                mappableProducts.Select(x => x.InternalID),
                opt => opt.WithStrictOrdering());
    }
}