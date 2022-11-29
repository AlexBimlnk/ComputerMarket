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

        var inputUser = TestHelper.GetOrdinaryUser();


        var expectedUser = new TUser[]
        {
            TestHelper.GetStorageUser(inputUser)
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

        var user = TestHelper.GetOrdinaryUser();

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

        var users = TestHelper.GetUsersCollection();

        await AddUserTypeAsync(UserType.Customer);
        await AddUserTypeAsync(UserType.Manager);
        await AddUserTypeAsync(UserType.Agent);

        users.ToList().ForEach(x => AddUserAsync(x).Wait());

        var repository = new UsersRepository(
            context.Object,
            logger);

        var inputUser = users.First();

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
            TestHelper.GetOrdinaryUser(),
            true
        },
        {
            TestHelper.GetOrdinaryUser(2, TestHelper.GetOrdinaryAuthenticationData("mail2@mail.ru", "login2")),
            false
        },
    };
}
