﻿using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TProvider = Logic.Storage.Models.Provider;

public class ProvidersRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProvidersRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger<ProvidersRepository>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProvidersRepository(
            context: null!,
            Mock.Of<ILogger<ProvidersRepository>>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProvidersRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can add provider.")]
    [Trait("Category", "Unit")]
    public async void CanAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var inputProvider = TestHelper.GetOrdinaryProvider();

        var storageProvider = TestHelper.GetStorageProvider(inputProvider);

        var providers = new Mock<DbSet<TProvider>>(MockBehavior.Strict);
        var providersCallback = 0;
        providers.Setup(x => x
            .AddAsync(
                It.Is<TProvider>(p =>
                    p.Id == storageProvider.Id &&
                    p.Name == storageProvider.Name &&
                    p.Margin == storageProvider.Margin &&
                    p.Inn == storageProvider.Inn &&
                    p.BankAccount == storageProvider.BankAccount),
                It.IsAny<CancellationToken>()))
            .Callback(() => providersCallback++)
            .Returns(new ValueTask<EntityEntry<TProvider>>());

        context.Setup(x => x.Providers)
            .Returns(providers.Object);

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await providersRepository.AddAsync(inputProvider));

        // Assert
        exception.Should().BeNull();
        providersCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot add provider when product is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddProductWhenProductIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await providersRepository.AddAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can cancel add provider.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var cts = new CancellationTokenSource();

        var providerRepository = new ProvidersRepository(
            context.Object,
            logger);

        var inputProvider = TestHelper.GetOrdinaryProvider();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await providerRepository.AddAsync(inputProvider, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot contains null provider.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsWhenProductIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await providersRepository.ContainsAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can cancel contains provider.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var cts = new CancellationTokenSource();

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        var inputProvider = TestHelper.GetOrdinaryProvider();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await providersRepository.ContainsAsync(inputProvider, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot delete null provider.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenProductIsNull()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            providersRepository.Delete(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can get provider by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var providerKey = new ID(1);
        var notExsistingProviderKey = new ID(2);

        var expectedResult = TestHelper.GetOrdinaryProvider(providerKey.Value);

        var data = new List<TProvider>
        {
            TestHelper.GetStorageProvider(expectedResult)
        }.AsQueryable();

        var providers = new Mock<DbSet<TProvider>>(MockBehavior.Strict);

        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Providers)
            .Returns(providers.Object);

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var result1 = providersRepository.GetByKey(providerKey);
        var result2 = providersRepository.GetByKey(notExsistingProviderKey);

        // Assert
        result1.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can get all products.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var expectedResult = new List<Provider>()
        {
            TestHelper.GetOrdinaryProvider()
        };

        var data = expectedResult
            .Select(x => TestHelper.GetStorageProvider(x))
            .AsQueryable();

        var providers = new Mock<DbSet<TProvider>>(MockBehavior.Strict);

        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        providers
            .As<IQueryable<TProvider>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Providers)
            .Returns(providers.Object);

        var productRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var result = productRepository.GetEntities();

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }


    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            providersRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
    }
}

