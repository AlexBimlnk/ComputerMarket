

using Market.Logic.Models;
using Market.Logic.Models.Abstractions;
using Market.Logic.Storage.Repositories;

using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Models;

public class CatalogTests
{
    [Fact(DisplayName = $"The {nameof(Catalog)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Catalog catalog = null!;
        var logger = Mock.Of<ILogger<Catalog>>();
        var productRepository = Mock.Of<IProductsRepository>();
        var itemRepository = Mock.Of<IItemsRepository>();
        

        // Act
        var exception = Record.Exception(() => catalog = new Catalog(productRepository, itemRepository, logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can not be created when logger is null.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithNullLogger()
    {
        // Arrange
        var productRepository = Mock.Of<IProductsRepository>();
        var itemRepository = Mock.Of<IItemsRepository>();

        // Act
        var exception = Record.Exception(() => _ = new Catalog(productRepository, itemRepository, logger: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can not be created when product repository is null.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithNullProductRepository()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Catalog>>();
        var itemRepository = Mock.Of<IItemsRepository>();

        // Act
        var exception = Record.Exception(() => _ = new Catalog(productRepository: null!, itemRepository, logger));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can not be created when item repository is null.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithNullItemRepository()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Catalog>>();
        var productRepository = Mock.Of<IProductsRepository>();

        // Act
        var exception = Record.Exception(() => _ = new Catalog(productRepository, itemRepository: null!, logger));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can get item types.")]
    [Trait("Category", "Unit")]
    public void CanGetItemTypes()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Catalog>>();
        var productRepository = Mock.Of<IProductsRepository>();
        var itemRepository = new Mock<IItemsRepository>(MockBehavior.Strict);

        var itemCollection = new Item[]
        {
            TestHelper.GetOrdinaryItem(1, TestHelper.GetOrdinaryItemType(1, "Name1")),
            TestHelper.GetOrdinaryItem(2, TestHelper.GetOrdinaryItemType(2, "Name2")),
            TestHelper.GetOrdinaryItem(3, TestHelper.GetOrdinaryItemType(1, "Name1")),
        };

        var itemRepositoryCallback = 0;
        itemRepository.Setup(x => x.GetEntities())
            .Returns(itemCollection)
            .Callback(() => itemRepositoryCallback++);

        var catalog = new Catalog(productRepository, itemRepository.Object, logger);

        var expectedResult = itemCollection
            .Select(x => x.Type)
            .DistinctBy(x => x.Id)
            .AsEnumerable();

        // Act
        var result = catalog.GetItemTypes();

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can filter by name.")]
    [Trait("Category", "Unit")]
    public void CanFilterByName()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Catalog>>();
        var itemRepository = Mock.Of<IItemsRepository>();
        var productRepository = new Mock<IProductsRepository>(MockBehavior.Strict);

        var item1 = TestHelper.GetOrdinaryItem(id: 1, name: "Name1");
        var item2 = TestHelper.GetOrdinaryItem(id: 2, name: "Name2");

        var products = new Product[]
        {
            TestHelper.GetOrdinaryProduct(item1),
            TestHelper.GetOrdinaryProduct(item2)
        };

        var repositoryCallback = 0;
        productRepository.Setup(x => x.GetEntities())
            .Returns(products)
            .Callback(() => repositoryCallback++);

        var searcshString1 = "Name2";
        var searcshString2 = "NotExsistingName";
        var catalogFilter1 = new CatalogFilter(searcshString1);
        var catalogFilter2 = new CatalogFilter(searcshString2);
        var expectedResult1 = products.Where(x => x.Item.Name == searcshString1).ToList();
        var expectedResult2 = Array.Empty<Product>();

        var catalog = new Catalog(productRepository.Object, itemRepository, logger);

        // Act
        var result1 = catalog.GetProducts(catalogFilter1);
        var result2 = catalog.GetProducts(catalogFilter2);

        // Arrange
        result1.Should().BeEquivalentTo(expectedResult1, opt => opt.WithStrictOrdering());
        result2.Should().BeEquivalentTo(expectedResult2, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can filter by type.")]
    [Trait("Category", "Unit")]
    public void CanFilterByType()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Catalog>>();
        var itemRepository = Mock.Of<IItemsRepository>();
        var productRepository = new Mock<IProductsRepository>(MockBehavior.Strict);

        var type1 = TestHelper.GetOrdinaryItemType(1, "Type1");
        var type2 = TestHelper.GetOrdinaryItemType(2, "Type2");

        var item1 = TestHelper.GetOrdinaryItem(id: 1, type: type1);
        var item2 = TestHelper.GetOrdinaryItem(id: 2, type: type2);

        var products = new Product[]
        {
            TestHelper.GetOrdinaryProduct(item1),
            TestHelper.GetOrdinaryProduct(item2)
        };

        var repositoryCallback = 0;
        productRepository.Setup(x => x.GetEntities())
            .Returns(products)
            .Callback(() => repositoryCallback++);

        var catalogFilter = new CatalogFilter(typeId: type2.Id);
        var expectedResult = products.Where(x => x.Item.Type.Id == type2.Id).ToList();

        var catalog = new Catalog(productRepository.Object, itemRepository, logger);

        // Act
        var result = catalog.GetProducts(catalogFilter);
        
        // Arrange
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(Catalog)} can filter by property.")]
    [Trait("Category", "Unit")]
    public void CanFilterByProperty()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Catalog>>();
        var itemRepository = Mock.Of<IItemsRepository>();
        var productRepository = new Mock<IProductsRepository>(MockBehavior.Strict);

        var properties1 = new ItemProperty[]
        {
            TestHelper.GetOrdinaryItemProperty(1, value: "Value1")
        };

        var properties2 = new ItemProperty[]
        {
            TestHelper.GetOrdinaryItemProperty(1, value: "Value2")
        };

        var values = new HashSet<(ID, string)>()
        {
            (new ID(1), "Value1")
        };

        var item1 = TestHelper.GetOrdinaryItem(id: 1, properties: properties1);
        var item2 = TestHelper.GetOrdinaryItem(id: 2, properties: properties2);

        var products = new Product[]
        {
            TestHelper.GetOrdinaryProduct(item1),
            TestHelper.GetOrdinaryProduct(item2)
        };

        var repositoryCallback = 0;
        productRepository.Setup(x => x.GetEntities())
            .Returns(products)
            .Callback(() => repositoryCallback++);


        var catalogFilter = new CatalogFilter(values: values);
        var expectedResult = new List<Product>()
        {
            TestHelper.GetOrdinaryProduct(item1)
        };

        var catalog = new Catalog(productRepository.Object, itemRepository, logger);

        // Act
        var result = catalog.GetProducts(catalogFilter);

        // Arrange
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithStrictOrdering());
    }
}
