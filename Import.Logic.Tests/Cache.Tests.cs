using Import.Logic.Models;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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
        var link = new Link(new ExternalID(1, Provider.Ivanov), new InternalID(2));
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

    [Fact(DisplayName = $"The {nameof(Cache)} cannot add already exsisting {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanNotAddExsistingLink()
    {
        // Arrange
        var link1 = new Link(new ExternalID(2, Provider.Ivanov), new InternalID(1));
        var link2 = new Link(new ExternalID(2, Provider.Ivanov), new InternalID(1));
        var cache = new Cache();
        cache.Add(link1);

        // Act
        var exception = Record.Exception(() =>
            cache.Add(link2));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can't add {nameof(Link)} with default value.")]
    [Trait("Category", "Unit")]
    public void CantAddLinkWithDefaultValue()
    {
        // Arrange
        var link = new Link(new ExternalID(1, Provider.Ivanov));
        var cache = new Cache();

        // Act
        var exception = Record.Exception(() =>
            cache.Add(link));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can add {nameof(Link)} range.")]
    [Trait("Category", "Unit")]
    public void CanAddLinkRange()
    {
        // Arrange
        var links = new Link[] 
        {
            new Link(new ExternalID(2, Provider.Ivanov), new InternalID(3)),
            new Link(new ExternalID(1, Provider.Ivanov), new InternalID(2)),
            new Link(new ExternalID(3, Provider.Ivanov), new InternalID(4))
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
            new Link(keys[0], new InternalID(5)),
            new Link(keys[1], new InternalID(6)),
            new Link(keys[2], new InternalID(7))
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
            new Link(new ExternalID(1, Provider.Ivanov), new InternalID(5)),
            new Link(new ExternalID(1, Provider.HornsAndHooves), new InternalID(6)),
            new Link(new ExternalID(4, Provider.HornsAndHooves), new InternalID(7))
        };
        var badlinks = new Link[]
        {
            new Link(new ExternalID(4, Provider.Ivanov), new InternalID(5)),
            new Link(new ExternalID(5, Provider.HornsAndHooves), new InternalID(6)),
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
            new Link(new ExternalID(1, Provider.Ivanov), new InternalID(5)),
            new Link(new ExternalID(1, Provider.HornsAndHooves), new InternalID(6)),
            new Link(new ExternalID(4, Provider.HornsAndHooves), new InternalID(7))
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
            new Link(new ExternalID(5,Provider.HornsAndHooves), new InternalID(3)),
            new Link(new ExternalID(4,Provider.HornsAndHooves), new InternalID(4))
        };

        var deletableLinks = new Link[]
        {
            new Link(new ExternalID(1,Provider.Ivanov), new InternalID(1)),
            new Link(new ExternalID(2,Provider.HornsAndHooves), new InternalID(2))
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
            new Link(new ExternalID(1, Provider.Ivanov), new InternalID(5)),
            new Link(new ExternalID(1, Provider.HornsAndHooves), new InternalID(6)),
            new Link(new ExternalID(4, Provider.HornsAndHooves), new InternalID(7))
        };

        Link linkFind = null!;
        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var exception = Record.Exception(() =>
            cache.Delete(linkFind));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can not delete not exsisting {nameof(Link)}.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteNotExsistinglLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new ExternalID(1, Provider.Ivanov), new InternalID(5)),
            new Link(new ExternalID(1, Provider.HornsAndHooves), new InternalID(6)),
            new Link(new ExternalID(4, Provider.HornsAndHooves), new InternalID(7))
        };

        var link = new Link(new ExternalID(2, Provider.Ivanov), new InternalID(9));
        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var exception = Record.Exception(() =>
            cache.Delete(link));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can get {nameof(Link)} by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByLink()
    {
        // Arrange
        var links = new Link[]
        {
            new Link(new ExternalID(5,Provider.HornsAndHooves), new InternalID(3)),
            new Link(new ExternalID(4,Provider.HornsAndHooves), new InternalID(4))
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
            new Link(new ExternalID(4,Provider.HornsAndHooves), new InternalID(4)),
            null!
        };

        var cache = new Cache();
        cache.AddRange(links);

        // Act
        var result = keys.Select(x => cache.GetByKey(x));

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithoutStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Cache)} can provide thread safe add.")]
    [Trait("Category", "Unit")]
    public async void CanProvideThreadSafeAddAsync()
    {
        // Arrange
        var provider1Links = Enumerable.Range(1, 100)
            .Select(x => new Link(new ExternalID(x, Provider.Ivanov), new InternalID(x)));
        var provider2Links = Enumerable.Range(2, 101)
            .Select(x => new Link(new ExternalID(x, Provider.HornsAndHooves), new InternalID(x)));

        var cache = new Cache();
        var mres = new ManualResetEventSlim();

        var t1 = Task.Run(() =>
        {
            mres.Wait();
            cache.AddRange(provider1Links);
        });
        var t2 = Task.Run(() =>
        {
            mres.Wait();
            cache.AddRange(provider2Links);
        });

        // Act
        mres.Set();
        await Task.WhenAll(t1,t2);

        // Assert
        provider1Links.Concat(provider2Links).Select(x => cache.Contains(x)).Should().AllBeEquivalentTo(true);
    }
}
