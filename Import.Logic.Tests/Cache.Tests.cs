using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Import.Logic.Tests;
public class CacheTests
{
    [Fact(DisplayName = $"The {nameof(Cache)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new Cache());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can add single {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanAddSingleLink()
    {
        // Arrange
        var externalId = new ExternalID(1, Provider.Ivanov);
        var internalId = new InternalID(2);
        var link = new Link(internalId, externalId);

        var cache = new Cache();

        // Act
        var exception = Record.Exception(() =>
            cache.Add(link));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can add {nameof(Link)} range.")]
    [Trait("Category", "Unit")]
    public void CanAddLinkRange()
    {
        // Arrange
        var links = new Link[] {
            new Link(new InternalID(2), new ExternalID(1, Provider.Ivanov)),
            new Link(new InternalID(3), new ExternalID(2, Provider.Ivanov)),
            new Link(new InternalID(4), new ExternalID(3, Provider.Ivanov))
        };
        
        var cache = new Cache();

        // Act
        var exception = Record.Exception(() =>
            cache.AddRange
            (links));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can determine containing of {nameof(Link)} by key.")]
    [Trait("Category", "Unit")]
    public void CanFindContainingOfLinkByKey()
    {
        // Arrange
        var keys = new ExternalID[]
        {
            new ExternalID(1, Provider.Ivanov),
            new ExternalID(1, Provider.HornsAndHooves),
            new ExternalID(4, Provider.HornsAndHooves)
        };

        var noneKeys = new ExternalID[]
        {
            new ExternalID(4, Provider.Ivanov),
            new ExternalID(5, Provider.HornsAndHooves),
        };

        var links = new Link[]
        {
            new Link(new InternalID(5), keys[0]),
            new Link(new InternalID(6), keys[1]),
            new Link(new InternalID(7), keys[2])
        };
        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var result = keys.Concat(noneKeys).Select(x => new {Result = cache.Contains(x), Key = x } );

        // Assert
        result.Join(keys, x => x.Key, k => k, (x,k) => x.Result).Should().AllBeEquivalentTo(true);
        result.Join(noneKeys, x => x.Key, k => k, (x,k) => x.Result).Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can determine containing of {nameof(Link)} by entity.")]
    [Trait("Category", "Unit")]
    public void CanFindContainingOfLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new InternalID(5), new ExternalID(1, Provider.Ivanov)),
            new Link(new InternalID(6), new ExternalID(1, Provider.HornsAndHooves)),
            new Link(new InternalID(7), new ExternalID(4, Provider.HornsAndHooves))
        };
        var badlinks = new Link[]
        {
            new Link(new InternalID(5), new ExternalID(4, Provider.Ivanov)),
            new Link(new InternalID(6), new ExternalID(5, Provider.HornsAndHooves)),
        };

        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var result = links.Concat(badlinks).Select(x => new { Result = cache.Contains(x), Link = x });

        // Assert
        result.Join(links, x => x.Link, k => k, (x, k) => x.Result).Should().AllBeEquivalentTo(true);
        result.Join(badlinks, x => x.Link, k => k, (x, k) => x.Result).Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can delete {nameof(Link)}s.")]
    [Trait("Category", "Unit")]
    public void CanDeleteLink()
    {
        //Arrange
        var links = new Link[2]
        {
            new Link(new InternalID(1), new ExternalID(1,Provider.Ivanov)),
            new Link(new InternalID(2), new ExternalID(2,Provider.HornsAndHooves)),
        };

        var cache = new Cache();



        //Act
        


        //Assert
        
    }
}
