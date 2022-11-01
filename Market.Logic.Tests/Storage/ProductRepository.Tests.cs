﻿using Market.Logic.Models;
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

public class ProductRepositoryTests
{
    [Fact(DisplayName = $"The {nameof(ProductRepository)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProductRepository(
            Mock.Of<IRepositoryContext>(MockBehavior.Strict),
            Mock.Of<ILogger<ProductRepository>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} cannot be created when repository context is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutRepositoryContext()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProductRepository(
            context: null!,
            Mock.Of<ILogger<ProductRepository>>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} cannot be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProductRepository(
            Mock.Of<IRepositoryContext>(),
            logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} can add product.")]
    [Trait("Category", "Unit")]
    public async void CanAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var storageProduct = new TProduct()
        {
            ItemId = 1,
            ProviderId = 1,
            Quantity = 2,
            ProviderCost = 100.00m
        };

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

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        var inputProduct = new Product(
            new Item(
                new ItemType("type1"),
                name: "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1),
            new Provider(
                name: "Provider1",
                new Margin(1.20m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"),
                id: 1),
            new Price(100.00m),
            quantity: 2);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddAsync(inputProduct));

        // Assert
        exception.Should().BeNull();
        productsCallback.Should().Be(1);
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} cannot add product when product is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotAddProductWhenProductIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} can cancel add product.")]
    [Trait("Category", "Unit")]
    public async void CanCancelAddProductAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var cts = new CancellationTokenSource();

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        var inputProduct = new Product(
            new Item(
                new ItemType("type1"),
                name: "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1),
            new Provider(
                name: "Provider1",
                new Margin(1.20m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"),
                id: 1),
            new Price(100.00m),
            quantity: 2);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.AddAsync(inputProduct, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} cannot contains product when product is null.")]
    [Trait("Category", "Unit")]
    public async void CanNotContainsWhenProductIsNullAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.ContainsAsync(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} can cancel contains product.")]
    [Trait("Category", "Unit")]
    public async void CanCancelContainsAsync()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var cts = new CancellationTokenSource();

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        var inputProduct = new Product(
            new Item(
                new ItemType("type1"),
                name: "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1),
            new Provider(
                name: "Provider1",
                new Margin(1.20m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"),
                id: 1),
            new Price(100.00m),
            quantity: 2);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await productRepository.ContainsAsync(inputProduct, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} can delete product.")]
    [Trait("Category", "Unit")]
    public void CanDeleteProduct()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var storageProduct = new TProduct()
        {
            ItemId = 1,
            ProviderId = 1,
            Quantity = 2,
            ProviderCost = 100.00m
        };

        var products = new Mock<DbSet<TProduct>>(MockBehavior.Loose);

        context.Setup(x => x.Products)
            .Returns(products.Object);

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        var containsProduct = new Product(
            new Item(
                new ItemType("type1"),
                name: "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1),
            new Provider(
                name: "Provider1",
                new Margin(1.20m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"),
                id: 1),
            new Price(100.00m),
            quantity: 2);

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

    [Fact(DisplayName = $"The {nameof(ProductRepository)} cannot delete product when product is null.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteWhenProductIsNull()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        // Act
        var exception = Record.Exception(() =>
            productRepository.Delete(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} can get product by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var data = new List<TProduct>
        {
            new TProduct()
            {
                ItemId = 1,
                ProviderId = 1,
                Quantity = 2,
                ProviderCost = 100.00m,
                Item = new TItem()
                {
                    Id = 1,
                    Name = "Name 1",
                    Description = Array.Empty<ItemDescription>(),
                        TypeId = 1,
                    Type = new TItemType()
                    {
                        Id = 1,
                        Name = "Type1"
                    }
                },
                Provider = new TProvider()
                {
                    Name = "Provider1",
                    Id = 1,
                    Inn = "1234512345",
                    Margin = 1.20m,
                    BankAccount = "12345123451234512345",
                }
            }
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

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        var expectedResult = new Product(
            new Item(
                new ItemType("Type1"),
                name: "Name 1",
                Array.Empty<ItemProperty>(),
                id: 1),
            new Provider(
                name: "Provider1",
                new Margin(1.20m),
                new PaymentTransactionsInformation(
                    "1234512345",
                    "12345123451234512345"),
                id: 1),
            new Price(100.00m),
            quantity: 2);

        // Act
        var result1 = productRepository.GetByKey((1, 1));
        var result2 = productRepository.GetByKey((1, 2));

        // Assert
        result1.Should().NotBeNull();
        result1.Should().BeEquivalentTo(expectedResult);
        result2.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ProductRepository)} can get all products.")]
    [Trait("Category", "Unit")]
    public void CanGetEntities()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var data = new List<TProduct>
        {
            new TProduct()
            {
                ItemId = 1,
                ProviderId = 1,
                Quantity = 2,
                ProviderCost = 100.00m,
                Item = new TItem()
                {
                    Id = 1,
                    Name = "Name 1",
                    Description = Array.Empty<ItemDescription>(),
                        TypeId = 1,
                    Type = new TItemType()
                    {
                        Id = 1,
                        Name = "Type1"
                    }
                },
                Provider = new TProvider()
                {
                    Name = "Provider1",
                    Id = 1,
                    Inn = "1234512345",
                    Margin = 1.20m,
                    BankAccount = "12345123451234512345",
                }
            }
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

        var productRepository = new ProductRepository(
            context.Object,
            logger);

        var expectedResult = new List<Product>()
        {
            new Product(
                new Item(
                    new ItemType("Type1"),
                    name: "Name 1",
                    Array.Empty<ItemProperty>(),
                    id: 1),
                new Provider(
                    name: "Provider1",
                    new Margin(1.20m),
                    new PaymentTransactionsInformation(
                        "1234512345",
                        "12345123451234512345"),
                    id: 1),
                new Price(100.00m),
                quantity: 2)
        };

        // Act
        var result = productRepository.GetEntities();

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }


    [Fact(DisplayName = $"The {nameof(ProductRepository)} can save.")]
    [Trait("Category", "Unit")]
    public void CanSave()
    {
        // Arrange
        var context = new Mock<IRepositoryContext>(MockBehavior.Loose);
        var logger = Mock.Of<ILogger<ProductRepository>>();

        var storageProduct = new TProduct()
        {
            ItemId = 1,
            ProviderId = 1,
            Quantity = 2,
            ProviderCost = 100.00m
        };

        var productRepository = new ProductRepository(
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

