﻿    using System.Net.WebSockets;

using General.Storage;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TProduct = Logic.Storage.Models.Product;
using ItemDescription = Logic.Storage.Models.ItemDescription;
using TItem = Logic.Storage.Models.Item;
using TItemType = Logic.Storage.Models.ItemType;
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

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot add null item .")]
    [Trait("Category", "Unit")]
    public async void CanNotAddNullItemAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var itemsRepository = new ProductsRepository(
            context.Object,
            logger);

        Item item = null!;

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await itemsRepository.AddAsync(item));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can cancel add item.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddItemAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var cts = new CancellationTokenSource();

        var itemRepository = new ProductsRepository(
            context.Object,
            logger);

        var inputItem = TestHelper.GetOrdinaryItem();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await itemRepository.AddAsync(inputItem, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot contains null item.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsNullItemAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var itemRepository = new ProductsRepository(
            context.Object,
            logger);

        Item item = null!;

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await itemRepository.ContainsAsync(item));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can cancel contains item.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var cts = new CancellationTokenSource();

        var itemsRepository = new ProductsRepository(
            context.Object,
            logger);

        var inputItem = TestHelper.GetOrdinaryItem();

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await itemsRepository.ContainsAsync(inputItem, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot delete null item.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenNullItem()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var itemsRepository = new ProductsRepository(
            context.Object,
            logger);

        Item item = null!;

        // Act
        var exception = Record.Exception(() =>
            itemsRepository.Delete(item));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can get item by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var expectedResult = TestHelper.GetOrdinaryItem();

        var data = new List<TItem>
        {
            TestHelper.GetStorageItem(expectedResult)
        }.AsQueryable();

        var items = new Mock<DbSet<TItem>>(MockBehavior.Strict);

        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Items)
            .Returns(items.Object);

        var itemsRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result1 = itemsRepository.GetByKey(expectedResult.Key);
        var result2 = itemsRepository.GetByKey(new ID(expectedResult.Key.Value + 1));

        // Assert
        result1.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can get all items.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var expectedResult = new List<Item>()
        {
            TestHelper.GetOrdinaryItem()
        };

        var data = expectedResult
            .Select(x => TestHelper.GetStorageItem(x))
            .AsQueryable();

        var items = new Mock<DbSet<TItem>>(MockBehavior.Strict);

        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.Provider)
            .Returns(data.Provider);
        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.Expression)
            .Returns(data.Expression);
        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.ElementType)
            .Returns(data.ElementType);
        items
            .As<IQueryable<TItem>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => data.GetEnumerator());

        context.Setup(x => x.Items)
            .Returns(items.Object);

        var itemsRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result = ((IRepository<Item>)itemsRepository).GetEntities();

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

        var itemsRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            itemsRepository.Save());

        // Assert
        exception.Should().BeNull();

        context.Verify(x =>
            x.SaveChanges(),
            Times.Once);
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

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Loose);

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
            await productRepository.AddOrUpdateAsync(inputProduct));

        // Assert
        exception.Should().BeNull();
        productsCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can update product.")]
    [Trait("Category", "Unit")]
    public async void CanUpdateProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var inputProduct = TestHelper.GetOrdinaryProduct(price: 100, quantity: 5);

        var oldStorageProduct = TestHelper.GetStorageProduct(TestHelper.GetOrdinaryProduct(price: 200, quantity: 6));
        var newStorageProduct = TestHelper.GetStorageProduct(inputProduct);

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Strict);
        
        var productsCallback = 0;

        products.Setup(x => x
            .FindAsync(
                It.Is<object[]>(ob =>
                    ob[0] is long &&
                    oldStorageProduct.ItemId == (long)ob[0] &&
                    ob[1] is long &&
                    oldStorageProduct.ProviderId == (long)ob[1]), It.IsAny<CancellationToken>()))
            .Callback(() => productsCallback++)
            .Returns(ValueTask.FromResult(oldStorageProduct));

        products.Setup(x => x
            .Update(
                It.Is<TProduct>(p =>
                    p.ItemId == newStorageProduct.ItemId &&
                    p.ProviderId == newStorageProduct.ProviderId &&
                    p.Quantity == newStorageProduct.Quantity &&
                    p.ProviderCost == newStorageProduct.ProviderCost)))
            .Callback(() => productsCallback++)
            .Returns(new ValueTask<EntityEntry<TProduct>>().Result);

        context.Setup(x => x.Products)
            .Returns(products.Object);

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddOrUpdateAsync(inputProduct));

        // Assert
        exception.Should().BeNull();
        productsCallback.Should().Be(2);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot add product when product is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddOrUpdateNullProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddOrUpdateAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can cancel add product.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddOrUpdateProductAsync()
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
            await productRepository.AddOrUpdateAsync(inputProduct, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can cancel contains product.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsProductAsync()
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

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} cannot remove null product.")]
    [Trait("Category", "Unit")]
    public void CanNotRemoveNullProduct()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var productRepository = new ProductsRepository(
            context.Object,
            logger);

        Product product = null!;

        // Act
        var exception = Record.Exception(() =>
            productRepository.Delete(product));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can get all products.")]
    [Trait("Category", "Unit")]
    public void CanGetProducts()
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
}

