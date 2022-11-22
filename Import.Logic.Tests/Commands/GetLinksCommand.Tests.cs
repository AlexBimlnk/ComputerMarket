using General.Logic.Commands;
using General.Logic.Executables;
using General.Storage;

using Import.Logic.Commands;
using Import.Logic.Models;
using Import.Logic.Queries;

using Moq;

namespace Import.Logic.Tests.Commands;

public class GetLinksCommandTests
{
    [Fact(DisplayName = $"The {nameof(GetLinksQuery)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        GetLinksQuery command = null!;
        var id = new ExecutableID("some id");
        var parameters = new GetLinksQueryParameters(id);
        var repository = Mock.Of<IRepository<Link>>();

        // Act
        var exception = Record.Exception(() =>
            command = new GetLinksQuery(parameters, repository));

        // Assert
        exception.Should().BeNull();
        command.Id.Should().Be(id);
    }

    [Fact(DisplayName = $"The {nameof(GetLinksQuery)} can't create without parameters.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutParameters()
    {
        // Arrange
        var repository = Mock.Of<IRepository<Link>>();

        // Act
        var exception = Record.Exception(() =>
            _ = new GetLinksQuery(parameters: null!, repository));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(GetLinksQuery)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutRepository()
    {
        // Arrange
        var id = new ExecutableID("some id");
        var parameters = new GetLinksQueryParameters(id);

        // Act
        var exception = Record.Exception(() =>
            _ = new GetLinksQuery(parameters, repository: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(GetLinksQuery)} can execute.")]
    [Trait("Category", "Unit")]
    public async void CanExecuteAsync()
    {
        // Arrange
        var id = new ExecutableID("some id");
        var parameters = new GetLinksQueryParameters(id);
        var links = new Link[]
        {
            new Link(new InternalID(1), new ExternalID(1, Provider.Ivanov)),
            new Link(new InternalID(2), new ExternalID(2, Provider.Ivanov)),
            new Link(new InternalID(3), new ExternalID(3, Provider.Ivanov)),
            new Link(new InternalID(1), new ExternalID(8, Provider.HornsAndHooves)),
            new Link(new InternalID(2), new ExternalID(9, Provider.HornsAndHooves))
        };

        var repository = new Mock<IRepository<Link>>(MockBehavior.Strict);

        var repositoryCallBack = 0;
        repository
            .Setup(x => x.GetEntities())
            .Returns(() => links)
            .Callback(() => repositoryCallBack++);

        var command = new GetLinksQuery(
            parameters,
            repository.Object);

        var expectedResult = QueryResult<IReadOnlyCollection<Link>>.Success(id, links);

        // Act
        var result = await command.ExecuteAsync();

        // Assert
        expectedResult.Should().BeEquivalentTo(result);
        repositoryCallBack.Should().Be(1);
    }
}

