﻿using Market.Logic.Models;
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

        var inputUser = TestHelper.GetOrdinaryUser();
        var storageUser = TestHelper.GetStorageUser(inputUser);

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

        var inputUser = TestHelper.GetOrdinaryUser();

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

        var inputUser = TestHelper.GetOrdinaryUser();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await userRepository.ContainsAsync(inputUser, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
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
        var userKey = new ID(1);
        var notUserKey = new ID(2);

        var data = new List<TUser>
        {
            TestHelper.GetStorageUser(TestHelper.GetOrdinaryUser(userKey.Value))
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

        var expectedResult = TestHelper.GetOrdinaryUser();

        // Act
        var result1 = userRepository.GetByKey(userKey);
        var result2 = userRepository.GetByKey(notUserKey);

        // Assert
        result1.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can get user by email.")]
    [Trait("Category", "Unit")]
    public void CanGetByEmail()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userEmail = "mail@mail.ru";
        var notExsistingUserEmail = "nonemail@mail.ru";
        
        var expectedResult = TestHelper.GetOrdinaryUser(data: TestHelper.GetOrdinaryAuthenticationData(email: userEmail));
        
        var data = new List<TUser>
        {
            TestHelper.GetStorageUser(expectedResult)
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

        var userEmail = "mail@mail.ru";
        var userPassword = "12345678";
        var inccorectUserPassword = "02345678";
        var notExsistingUserEmail = "nonemail@mail.ru";

        var expectedResult = TestHelper.GetOrdinaryUser(data: TestHelper.GetOrdinaryAuthenticationData(email: userEmail, pass: userPassword));
        
        var data = new List<TUser>
        {
            TestHelper.GetStorageUser(TestHelper.GetOrdinaryUser(data: TestHelper.GetOrdinaryAuthenticationData(email: userEmail, pass: userPassword)))
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

        // Act
        var result1 = userRepository.IsCanAuthenticate(
            TestHelper.GetOrdinaryAuthenticationData(
                email: userEmail, 
                login: null, pass: 
                userPassword), 
            out var user1);

        var result2 = userRepository.IsCanAuthenticate(
            TestHelper.GetOrdinaryAuthenticationData(
                email: userEmail, 
                login: null, 
                pass: inccorectUserPassword), 
            out var user2);

        var result3 = userRepository.IsCanAuthenticate(
            TestHelper.GetOrdinaryAuthenticationData(
                email: notExsistingUserEmail, 
                login: null,
                pass: userPassword), 
            out var user3);

        var result4 = userRepository.IsCanAuthenticate(
            TestHelper.GetOrdinaryAuthenticationData(
                email: notExsistingUserEmail, 
                login: null, 
                pass: inccorectUserPassword), 
            out var user4);

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

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can define matching of password.")]
    [Trait("Category", "Unit")]
    public void CanDefineMatchingPassword()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var userPassword = "12345678";
        var inccorectUserPassword = "02345678";
        var userId = new ID(1);
        var notExsistingUserId = new ID(2);

        var user = TestHelper.GetOrdinaryUser(userId.Value, data: TestHelper.GetOrdinaryAuthenticationData(pass: userPassword));
        
        var data = new List<TUser>
        {
            TestHelper.GetStorageUser(user)
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

        // Act
        var result1 = userRepository.IsPasswordMatch(userId, new Password(userPassword));
        var result2 = userRepository.IsPasswordMatch(userId, new Password(inccorectUserPassword));
        var result3 = userRepository.IsPasswordMatch(notExsistingUserId, new Password(userPassword));

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeFalse();
        result3.Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(UsersRepository)} can get all users.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<UsersRepository>>();

        var expectedResult = new List<User>()
        {
            TestHelper.GetOrdinaryUser()
        };

        var data = expectedResult
            .Select(x => TestHelper.GetStorageUser(x))
            .AsQueryable();

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