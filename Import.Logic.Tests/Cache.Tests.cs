using Import.Logic.Models;

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
        var link = new Link(new InternalID(2), new ExternalID(1, Provider.Ivanov));
        var cache = new Cache();

        // Act
        var exception = Record.Exception(() =>
            cache.Add(link));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} cannot add null {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanNotAddNull()
    {
        // Arrange
        var cache = new Cache();

        // Act
        var exception = Record.Exception(() =>
            cache.Add(entity: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
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
            cache.AddRange(links));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} cannot add null {nameof(Link)} range.")]
    [Trait("Category", "Unit")]
    public void CanNotAddNullRange()
    {
        // Arrange
        var cache = new Cache();

        // Act
        var exception = Record.Exception(() =>
            cache.AddRange(entities: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can find {nameof(Link)} by key.")]
    [Trait("Category", "Unit")]
    public void CanFindLinkByKey()
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
        var goodResult = keys.Select(x => cache.Contains(x));
        var badResult = noneKeys.Select(x => cache.Contains(x));

        // Assert
        goodResult.Should().AllBeEquivalentTo(true);
        badResult.Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can find {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanFindLink()
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
        var resultGood = links.Select(x => cache.Contains(x));
        var resultBad = badlinks.Select(x => cache.Contains(x));

        // Assert
        resultGood.Should().AllBeEquivalentTo(true);
        resultBad.Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can not find null {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanNotFindNullLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new InternalID(5), new ExternalID(1, Provider.Ivanov)),
            new Link(new InternalID(6), new ExternalID(1, Provider.HornsAndHooves)),
            new Link(new InternalID(7), new ExternalID(4, Provider.HornsAndHooves))
        };

        Link linkFind = null!;
        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var exception = Record.Exception(() =>
            cache.Contains(linkFind));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can delete {nameof(Link)}s.")]
    [Trait("Category", "Unit")]
    public void CanDeleteLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new InternalID(3), new ExternalID(5,Provider.HornsAndHooves)),
            new Link(new InternalID(4), new ExternalID(4,Provider.HornsAndHooves))
        };

        var deletableLinks = new Link[]
        {
            new Link(new InternalID(1), new ExternalID(1,Provider.Ivanov)),
            new Link(new InternalID(2), new ExternalID(2,Provider.HornsAndHooves))
        };

        var cache = new Cache();
        cache.AddRange(links.Concat(deletableLinks));

        // Act
        cache.Delete(deletableLinks[0]);
        cache.Delete(deletableLinks[1]);

        // Assert
        links.Select(x => cache.Contains(x)).Should().AllBeEquivalentTo(true);
        deletableLinks.Select(x => cache.Contains(x)).Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can not delete null {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteNullLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new InternalID(5), new ExternalID(1, Provider.Ivanov)),
            new Link(new InternalID(6), new ExternalID(1, Provider.HornsAndHooves)),
            new Link(new InternalID(7), new ExternalID(4, Provider.HornsAndHooves))
        };

        Link linkFind = null!;
        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var exception = Record.Exception(() =>
            cache.Delete
            (linkFind));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can get {nameof(Link)} by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new InternalID(3), new ExternalID(5,Provider.HornsAndHooves)),
            new Link(new InternalID(4), new ExternalID(4,Provider.HornsAndHooves))
        };

        var keys = new ExternalID[]
        {
            new ExternalID(5,Provider.HornsAndHooves),
            new ExternalID(5,Provider.Ivanov),
            new ExternalID(4,Provider.HornsAndHooves),
            new ExternalID(1,Provider.HornsAndHooves)
        };

        var expectedResult = new Link?[]
        {
            links[0],
            null!,
            new Link(new InternalID(4), new ExternalID(4,Provider.HornsAndHooves)),
            null!
        };

        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var result = keys.Select(x => cache.GetByKey(x));

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
