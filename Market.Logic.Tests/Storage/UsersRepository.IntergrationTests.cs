using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TUser = Logic.Storage.Models.User;

public class UsersRepositoryIntegrationTests : DBIntegrationTestBase
{
    public UsersRepositoryIntegrationTests()
        : base(nameof(UsersRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can add user.")]
    [Trait("Category", "Integration")]
    public async Task CanAddUserAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);

        await AddUserTypeAsync(UserType.Customer);

        var inputUser = new User(
            id: new InternalID(1),
            new AuthenticationData(
                email: "mmail@mail.ru",
                new Password("12345678"),
                login: "Login1"),
            UserType.Customer);


        var expectedUser = new TUser[]
        {
            new TUser
            {
                Id = 1,
                UserTypeId = 1,
                Login = "Login1",
                Email = "mmail@mail.ru",
                Password = "12345678"
            }
        };

        var repository = new UsersRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputUser)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbUsers = await GetTableRecordsAsync(
            "users",
            r => new TUser
            {
                Id = r.GetInt64(0),
                Login = r.GetString(1),
                Password = r.GetString(3),
                Email = r.GetString(2),
                UserTypeId = r.GetInt16(4),
            });

        // Assert
        dbUsers.Should().BeEquivalentTo(expectedUser);
    }

    [Theory(DisplayName = $"The {nameof(UsersRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(User inputUser, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);

        var user = new User(
            id: new InternalID(1),
            new AuthenticationData(
                email: "mmail1@mail.ru",
                new Password("12345678"),
                login: "Login1"),
            UserType.Customer);

        await AddUserTypeAsync(UserType.Customer);

        await AddUserAsync(user);

        var repository = new UsersRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputUser)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can delete user.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteUserAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Users)
            .Returns(_marketContext.Users);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var user1 = new User(
            id: new InternalID(1),
            new AuthenticationData(
                email: "mmail1@mail.ru",
                new Password("12345678"),
                login: "Login1"),
            UserType.Customer);

        var user2 = new User(
            id: new InternalID(4),
            new AuthenticationData(
                email: "smail@mail.ru",
                new Password("12345679"),
                login: "Login2"),
            UserType.Customer);


        await AddUserTypeAsync(UserType.Customer);

        await AddUserAsync(user1);
        await AddUserAsync(user2);

        var repository = new UsersRepository(
            context.Object,
            logger);

        var inputUser = new User(
            id: new InternalID(1),
            new AuthenticationData(
                email: "mmail@mail.ru",
                new Password("12345678"),
                login: "Login1"),
            UserType.Customer);

        // Act
        var beforeContains = await repository.ContainsAsync(inputUser)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputUser);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputUser)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
    }

    private async Task AddUserAsync(User user)
    {
        var fromQuery = "users (id, login, password, email, user_type_id)";
        var valuesQuery =
            $"({user.Key.Value}, " +
            $"'{user.AuthenticationData.Login}', " +
            $"'{user.AuthenticationData.Password.Value}', " +
            $"'{user.AuthenticationData.Email}', " +
            $"{(short)user.Type})";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddUserTypeAsync(UserType type)
    {
        var fromQuery = "user_type (id, name)";
        var valuesQuery = $"({(short)type}, '{type}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    public static readonly TheoryData<User, bool> ContainsData = new()
    {
        {
            new(
                id: new InternalID(1),
                new AuthenticationData(
                    email: "mmail1@mail.ru",
                    new Password("12345678"),
                    login: "Login1"),
                UserType.Customer),
            true
        },
        {
            new(
                id: new InternalID(4),
                new AuthenticationData(
                    email: "tmail2@mail.ru",
                    new Password("12345678"),
                    login: "Login2"),
                UserType.Customer),
            false
        },
    };
}
