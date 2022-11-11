using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TUser = Logic.Storage.Models.User;

public class UsersRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(UsersRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new UsersRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger<UsersRepository>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new UsersRepository(
            context: null!,
            Mock.Of<ILogger<UsersRepository>>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new UsersRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can add user.")]
    [Trait("Category", "Unit")]
    public async void CanAddUserAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var storageUser = new TUser()
        {
            Id = 1,
            UserTypeId = 1,
            Login = "Login1",
            Email = "mmail@mail.ru",
            Password = "12345678"
        };

        var users = new Mock<DbSet<TUser>>(MockBehavior.Strict);
        var usersCallback = 0;
        users.Setup(x => x
            .AddAsync(
                It.Is<TUser>(p =>
                    p.UserTypeId == storageUser.UserTypeId &&
                    p.Login == storageUser.Login &&
                    p.Email == storageUser.Email &&
                    p.Password == storageUser.Password),
                It.IsAny<CancellationToken>()))
            .Callback(() => usersCallback++)
            .Returns(new ValueTask<EntityEntry<TUser>>());

        context.Setup(x => x.Users)
            .Returns(users.Object);

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var inputUser = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: "mmail@mail.ru",
                new Password("12345678")),
            UserType.Customer);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await userRepository.AddAsync(inputUser));

        // Assert
        exception.Should().BeNull();
        usersCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} cannot add user when user is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddUserWhenUserIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await userRepository.AddAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can cancel add user.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddUserAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var cts = new CancellationTokenSource();

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var inputUser = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: "mail@mail.ru",
                new Password("12345678")),
            UserType.Customer);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await userRepository.AddAsync(inputUser, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} cannot contains user when user is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsWhenUserIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await userRepository.ContainsAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can cancel contains user.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var cts = new CancellationTokenSource();

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var inputUser = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: "mail@mail.ru",
                new Password("12345678")),
            UserType.Customer);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await userRepository.ContainsAsync(inputUser, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can delete user.")]
    [Trait("Category", "Unit")]
    public void CanDeleteUser()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var storageUser = new TUser()
        {
            UserTypeId = 1,
            Login = "Login1",
            Email = "mail@mail.ru",
            Password = "12345678"
        };

        var users = new Mock<DbSet<TUser>>(MockBehavior.Loose);

        context.Setup(x => x.Users)
            .Returns(users.Object);

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var containsUser = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: "mail@mail.ru",
                new Password("12345678")),
            UserType.Customer);

        // Act
        var exception = Record.Exception(() =>
            userRepository.Delete(containsUser));

        // Assert
        exception.Should().BeNull();

        users.Verify(x =>
            x.Remove(
                It.Is<TUser>(p =>
                    p.UserTypeId == storageUser.UserTypeId &&
                    p.Login == storageUser.Login &&
                    p.Email == storageUser.Email &&
                    p.Password == storageUser.Password)),
            Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} cannot delete user when user is null.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenUserIsNull()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            userRepository.Delete(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can get user by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var data = new List<TUser>
        {
            new TUser()
            {
                Id = 1,
                UserTypeId = 1,
                Login = "Login1",
                Email = "mmail@mail.ru",
                Password = "12345678"
            }
        }.AsQueryable();

        var users = new Mock<DbSet<TUser>>(MockBehavior.Strict);

        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Users)
            .Returns(users.Object);

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var expectedResult = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: "mmail@mail.ru",
                new Password("12345678")),
            UserType.Customer);

        // Act
        var result1 = userRepository.GetByKey(new InternalID(1));
        var result2 = userRepository.GetByKey(new InternalID(2));

        // Assert
        result1.Should().NotBeNull();
        result1.Should().BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can get user by email.")]
    [Trait("Category", "Unit")]
    public void CanGetByEmail()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userEmail = "mmail@mail.ru";
        var notExsistingUserEmail = "amail@mail.ru";

        var data = new List<TUser>
        {
            new TUser()
            {
                Id = 1,
                UserTypeId = 1,
                Login = "Login1",
                Email = userEmail,
                Password = "12345678"
            }
        }.AsQueryable();

        var users = new Mock<DbSet<TUser>>(MockBehavior.Strict);

        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Users)
            .Returns(users.Object);

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var expectedResult = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: userEmail,
                new Password("12345678")),
            UserType.Customer);

        // Act
        var result1 = userRepository.GetByEmail(userEmail);
        var result2 = userRepository.GetByEmail(notExsistingUserEmail);

        // Assert
        result1.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can authenticate user by login data.")]
    [Trait("Category", "Unit")]
    public void CanAuthenticateUserByLoginData()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userEmail = "mmail@mail.ru";
        var userPassword = "12345678";
        var inccorectUserPassword = "abcdefgk";
        var notExsistingUserEmail = "amail@mail.ru";

        var data = new List<TUser>
        {
            new TUser()
            {
                Id = 1,
                UserTypeId = 1,
                Login = "Login1",
                Email = userEmail,
                Password = userPassword
            }
        }.AsQueryable();

        var users = new Mock<DbSet<TUser>>(MockBehavior.Strict);

        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Users)
            .Returns(users.Object);

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var expectedResult = new User(
            new InternalID(1),
            new AuthenticationData(
                login: "Login1",
                email: userEmail,
                new Password(userPassword)),
            UserType.Customer);

        // Act
        var result1 = userRepository.IsCanAuthenticate(userEmail, userPassword, out var user1);
        var result2 = userRepository.IsCanAuthenticate(userEmail, inccorectUserPassword, out var user2);
        var result3 = userRepository.IsCanAuthenticate(notExsistingUserEmail, userPassword, out var user3);
        var result4 = userRepository.IsCanAuthenticate(notExsistingUserEmail, inccorectUserPassword, out var user4);
        

        // Assert
        result1.Should().BeTrue();
        user1.Should().BeEquivalentTo(expectedResult);
        result2.Should().BeFalse();
        user2.Should().BeNull();
        result3.Should().BeFalse();
        user3.Should().BeNull();
        result4.Should().BeFalse();
        user4.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can get all users.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var data = new List<TUser>
        {
            new TUser()
            {
                Id = 1,
                UserTypeId = 1,
                Login = "Login1",
                Email = "mmail@mail.ru",
                Password = "12345678"
            }
        }.AsQueryable();

        var users = new Mock<DbSet<TUser>>(MockBehavior.Strict);

        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        users
            .As<IQueryable<TUser>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Users)
            .Returns(users.Object);

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        var expectedResult = new List<User>()
        {
            new User(
                new InternalID(1),
                new AuthenticationData(
                    login: "Login1",
                    email: "mmail@mail.ru",
                    new Password("12345678")),
                UserType.Customer)
        };

        // Act
        var result = userRepository.GetEntities();

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }


    [Fact(DisplayName = $"The {nameof(UsersRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var storageUser = new TUser()
        {
            Id = 1,
            UserTypeId = 1,
            Login = "Login1",
            Email = "mmail@mail.ru",
            Password = "12345678"
        };

        var userRepository = new UsersRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            userRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
    }
}