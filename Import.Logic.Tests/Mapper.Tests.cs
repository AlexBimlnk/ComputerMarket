using Castle.Core.Logging;

using Import.Logic.Abstractions;
using Import.Logic.Commands;
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
        var cache = new Mock<ICache<ExternalID, Link>>();

        // Act
        var exception = Record.Exception(() => 
            _ = new Mapper(cache.Object));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can't create without cache.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutCache()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new Mapper(cache: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Mapper)} can map single {nameof(Product)}.")]
    [Trait("Category", "Unit")]
    public void CanMapSingleProduct()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);
        var internalId = new InternalID(2);
        
        var link = new Link(internalId, externalId);

        var cache = new Mock<ICache<ExternalID, Link>>();

        cache.Setup(x => x.Contains(externalId))
            .Returns(false);

        cache.Setup(x => x.GetByKey(externalId))
            .Returns(link);

        var mapper = new Mapper(cache.Object);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        cacheInvokeCount.Should().Be(1);

        cache.Verify(x => x.Add(link.ExternalID, link), Times.Once);

        repository.Verify(x => x.AddAsync(link), Times.Once);
        repository.Verify(x => x.SaveAsync(), Times.Once);
    }
}