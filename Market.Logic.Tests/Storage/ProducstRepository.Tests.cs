using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using ItemDescription = Logic.Storage.Models.ItemDescription;
using TItem = Logic.Storage.Models.Item;
using TItemType = Logic.Storage.Models.ItemType;
using TProduct = Logic.Storage.Models.Product;
using TProvider = Logic.Storage.Models.Provider;

public class ProductsRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProductsRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger<ProductsRepository>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProductsRepository(
            context: null!,
            Mock.Of<ILogger<ProductsRepository>>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProductsRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can add product.")]
    [Trait("Category", "Unit")]
    public async void CanAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var inputProduct = TestHelper.GetOrdinaryProduct();

        var storageProduct = TestHelper.GetStorageProduct(inputProduct);

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Strict);
        var productsCallback = 0;
        products.Setup(x => x
            .AddAsync(
                It.Is<TProduct>(p =>
                    p.ItemId == storageProduct.ItemId &&
                    p.ProviderId == storageProduct.ProviderId &&
                    p.Quantity == storageProduct.Quantity &&
                    p.ProviderCost == storageProduct.ProviderCost),
                It.IsAny<CancellationToken>()))
            .Callback(() => productsCallback++)
            .Returns(new ValueTask<EntityEntry<TProduct>>());

        context.Setup(x => x.Products)
            .Returns(products.Object);

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddAsync(inputProduct));

        // Assert
        exception.Should().BeNull();
        productsCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot add product when product is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddProductWhenProductIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can cancel add product.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var cts = new CancellationTokenSource();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        var inputProduct = TestHelper.GetOrdinaryProduct();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddAsync(inputProduct, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot contains product when product is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsWhenProductIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.ContainsAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can cancel contains product.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var cts = new CancellationTokenSource();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        var inputProduct = TestHelper.GetOrdinaryProduct();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.ContainsAsync(inputProduct, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can delete product.")]
    [Trait("Category", "Unit")]
    public void CanDeleteProduct()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var containsProduct = TestHelper.GetOrdinaryProduct();

        var storageProduct = TestHelper.GetStorageProduct(containsProduct);

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Loose);

        context.Setup(x => x.Products)
            .Returns(products.Object);

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            productRepository.Delete(containsProduct));

        // Assert
        exception.Should().BeNull();

        products.Verify(x =>
            x.Remove(
                It.Is<TProduct>(p =>
                    p.ItemId == storageProduct.ItemId &&
                    p.ProviderId == storageProduct.ProviderId &&
                    p.Quantity == storageProduct.Quantity &&
                    p.ProviderCost == storageProduct.ProviderCost)),
            Times.Once);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot delete product when product is null.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenProductIsNull()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            productRepository.Delete(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can get product by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var expectedResult = TestHelper.GetOrdinaryProduct();

        var data = new List<TProduct>
        {
            TestHelper.GetStorageProduct(expectedResult)
        }.AsQueryable();

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Strict);

        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Products)
            .Returns(products.Object);

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result1 = productRepository.GetByKey((expectedResult.Key.Item1, expectedResult.Key.Item2));
        var result2 = productRepository.GetByKey((expectedResult.Key.Item1, expectedResult.Key.Item2 + 1));

        // Assert
        result1.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can get all products.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var expectedResult = new List<Product>()
        {
            TestHelper.GetOrdinaryProduct()
        };

        var data = expectedResult
            .Select(x => TestHelper.GetStorageProduct(x))
            .AsQueryable();

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Strict);

        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        products
            .As<IQueryable<TProduct>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Products)
            .Returns(products.Object);

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result = productRepository.GetEntities();

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }


    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            productRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
    }
}

