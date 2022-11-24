using General.Logic.Executables;
using General.Logic.Queries;

using Import.Logic.Commands;
using Import.Logic.Models;
using Import.Logic.Queries;

using Moq;

namespace Import.Logic.Tests.Commands;

public class QueryFactoryTests
{
    [Fact(DisplayName = $"The {nameof(QueryFactory)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var getLinks = Mock.Of<Func<GetLinksQueryParameters, IQuery>>();
        
        // Act
        var exception = Record.Exception(() =>
            _ = new QueryFactory(getLinks));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(QueryFactory)} can create get links command.")]
    [Trait("Category", "Unit")]
    public void CanCreateGetLinksCommand()
    {
        // Arrange
        var getLinksParameters = new GetLinksQueryParameters(
            new("some id"));

        var query = Mock.Of<IQuery>(MockBehavior.Strict);

        var getLinks = new Mock<Func<GetLinksQueryParameters, IQuery>>(MockBehavior.Strict);

        var getLinksInvokeCount = 0;
        getLinks.Setup(x => x.Invoke(getLinksParameters))
            .Returns(query)
            .Callback(() => getLinksInvokeCount++);

        var factory = new QueryFactory(getLinks.Object);

        // Act
        IQuery result = null!;

        var exception = Record.Exception(() =>
            result = factory.Create(getLinksParameters));

        // Assert
        exception.Should().BeNull();
        result.Should().Be(query);
        getLinksInvokeCount.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(QueryFactory)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutParameters()
    {
        // Arrange
        var getLinks = Mock.Of<Func<GetLinksQueryParameters, IQuery>>();

        var factory = new QueryFactory(getLinks);

        // Act
        var exception = Record.Exception(() =>
            _ = factory.Create(parameters: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(QueryFactory)} can't create with unknown parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithUnknownParameters()
    {
        // Arrange
        var getLinks = Mock.Of<Func<GetLinksQueryParameters, IQuery>>();
        var unknownParameters = new UnknownParameters(new("some id"));

        var factory = new QueryFactory(getLinks);

        // Act
        var exception = Record.Exception(() =>
            _ = factory.Create(unknownParameters));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    private class UnknownParameters : QueryParametersBase
    {
        public UnknownParameters(ExecutableID id) : base(id)
        {
        }
    }
}
