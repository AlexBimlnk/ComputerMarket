using Market.Logic.Models;
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

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can add product.")]
    [Trait("Category", "Unit")]
    public async void CanAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var storageProvider = new TProvider()
        {
            Id = 1,
            Name = "Provider Name 1",
            Margin = 1.3m,
            Inn = "1234512345", 
            BankAccount = "12345123451234512345"
        };

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

        var inputProvider = new Provider(
                id: new ID(1),
                name: "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"));

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await providersRepository.AddAsync(inputProvider));

        // Assert
        exception.Should().BeNull();
        providersCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot add product when product is null.")]
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

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can cancel add product.")]
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

        var inputProvider = new Provider(
                id: new ID(1),
                name: "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"));

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await providerRepository.AddAsync(inputProvider, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot contains product when product is null.")]
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

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can cancel contains product.")]
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

        var inputProvider = new Provider(
                id: new ID(1),
                name: "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"));

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await providersRepository.ContainsAsync(inputProvider, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can delete product.")]
    [Trait("Category", "Unit")]
    public void CanDeleteProduct()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var storageProvider = new TProvider()
        {
            Id = 1,
            Name = "Provider Name 1",
            Margin = 1.3m,
            Inn = "1234512345",
            BankAccount = "12345123451234512345"
        };

        var providers = new Mock<DbSet<TProvider>>(MockBehavior.Loose);

        context.Setup(x => x.Providers)
            .Returns(providers.Object);

        var providersRepository = new ProvidersRepository(
            context.Object,
            logger);

        var containsProvider = new Provider(
                id: new ID(1),
                name: "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"));

        // Act
        var exception = Record.Exception(() =>
            providersRepository.Delete(containsProvider));

        // Assert
        exception.Should().BeNull();

        providers.Verify(x =>
            x.Remove(
                It.Is<TProvider>(p =>
                    p.Id == storageProvider.Id &&
                    p.Name == storageProvider.Name &&
                    p.Margin == storageProvider.Margin&&
                    p.Inn == storageProvider.Inn &&
                    p.BankAccount == storageProvider.BankAccount)),
            Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} cannot delete product when product is null.")]
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

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can get product by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var data = new List<TProvider>
        {
            new TProvider()
            {
                Id = 1,
                Name = "Provider Name 1",
                Margin = 1.3m,
                Inn = "1234512345",
                BankAccount = "12345123451234512345"
            }
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

        var expectedResult = new Provider(
                id: new ID(1),
                name: "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"));

        // Act
        var result1 = providersRepository.GetByKey(new ID(1));
        var result2 = providersRepository.GetByKey(new ID(2));

        // Assert
        result1.Should().NotBeNull();
        result1.Should().BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProvidersRepository)} can get all products.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProvidersRepository>>();

        var data = new List<TProvider>
        {
            new TProvider()
            {
                Id = 1,
                Name = "Provider Name 1",
                Margin = 1.3m,
                Inn = "1234512345",
                BankAccount = "12345123451234512345"
            }
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

        var productRepository = new ProvidersRepository(
            context.Object,
            logger);

        var expectedResult = new List<Provider>()
        {
            new Provider(
                id: new ID(1),
                name: "Provider Name 1",
                new Margin(1.3m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"))
        };

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

        var storageProduct = new TProvider()
        {
            Id = 1,
            Name = "Provider Name 1",
            Margin = 1.3m,
            Inn = "1234512345",
            BankAccount = "12345123451234512345"
        };

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

