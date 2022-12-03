using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests.Storage;

using TProduct = Logic.Storage.Models.Product;
using TItem = Logic.Storage.Models.Item;
using TItemDescription = Logic.Storage.Models.ItemDescription;

public class ProductsRepositoryIntegrationTests : DBIntegrationTestBase
{
    public ProductsRepositoryIntegrationTests()
        : base(nameof(ProductsRepositoryIntegrationTests)) { }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can add item.")]
    [Trait("Category", "Integration")]
    public async Task CanAddItemAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Items)
            .Returns(_marketContext.Items);

        context.SetupGet(x => x.ItemTypes)
            .Returns(_marketContext.ItemTypes);

        context.SetupGet(x => x.PropertyGroups)
            .Returns(_marketContext.PropertyGroups);

        context.SetupGet(x => x.ItemProperties)
            .Returns(_marketContext.ItemProperties);

        var group = TestHelper.GetOrdinaryPropertyGroup();

        var properties = new ItemProperty[]
        {
            TestHelper.GetOrdinaryItemProperty(1, "PropName1", "Value1", group),
            TestHelper.GetOrdinaryItemProperty(2, "PropName1", "Value2", group),
            TestHelper.GetOrdinaryItemProperty(3, "PropName1", "Value3", group)
        };

        var type = TestHelper.GetOrdinaryItemType();

        await AddPropertyGroupAsync(group);

        await AddItemTypeAsync(type);

        await Task.WhenAll(properties.Select(async x => AddItemPropertyAsync(x)).ToArray());

        var inputItem = TestHelper.GetOrdinaryItem(type: type, properties: properties);

        var expectedItem = new TItem[]
        {
            TestHelper.GetStorageItem(inputItem)
        }
        .Select(x => { x.Description = null!; x.Type = null!; return x; })
        .ToArray();

        var expectedDescription = properties
            .Select(x => TestHelper.GetStorageItemPropertyWithOutItem(x))
            .Select(x => { x.ItemId = inputItem.Key.Value; x.Property = null!; return x; })
            .ToArray();

        var repository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddAsync(inputItem)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbItems = await GetTableRecordsAsync(
            "items",
            r => new TItem
            {
                Id = r.GetInt64(0),
                Name = r.GetString(1),
                TypeId = r.GetInt32(2),
                Description = null!,
                Type = null!
            });

        var dbDescription = await GetTableRecordsAsync(
            "item_description",
            r => new TItemDescription
            {
                ItemId = r.GetInt64(0),
                PropertyId = r.GetInt64(1),
                PropertyValue = r.GetString(2),
                Property = null!,
                Item = null!
            });

        // Assert
        dbItems.Should().BeEquivalentTo(expectedItem);
        dbDescription.Should().BeEquivalentTo(expectedDescription);
    }

    [Theory(DisplayName = $"The {nameof(ProductsRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsData))]
    public async Task CanContainsWorksAsync(Item inputItem, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Items)
            .Returns(_marketContext.Items);

        var group = TestHelper.GetOrdinaryPropertyGroup();

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Name1");

        await AddPropertyGroupAsync(group);

        await AddItemTypeAsync(type);

        await Task.WhenAll(item.Properties
            .Select(async x => AddItemPropertyAsync(x))
            .ToArray());

        await AddItemAsync(item);

        var repository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputItem)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can delete item.")]
    [Trait("Category", "Integration")]
    public async Task CanDeleteItemsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Items)
            .Returns(_marketContext.Items);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var group = TestHelper.GetOrdinaryPropertyGroup();

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Name1");

        await AddPropertyGroupAsync(group);

        await AddItemTypeAsync(type);

        await Task.WhenAll(item.Properties
            .Select(async x => AddItemPropertyAsync(x))
            .ToArray());

        await AddItemAsync(item);

        var repository = new ProductsRepository(
            context.Object,
            logger);

        var inputItem = TestHelper.GetOrdinaryItem(1, type, "Name1");

        // Act
        var beforeContains = await repository.ContainsAsync(inputItem)
            .ConfigureAwait(false);

        var exception = Record.Exception(() =>
        {
            repository.Delete(inputItem);
            repository.Save();
        });

        var afterContains = await repository.ContainsAsync(inputItem)
            .ConfigureAwait(false);

        // Assert
        beforeContains.Should().BeTrue();
        exception.Should().BeNull();
        afterContains.Should().BeFalse();
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can update item.")]
    [Trait("Category", "Integration")]
    public async Task CanUdpateItemAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Items)
            .Returns(_marketContext.Items);

        context.SetupGet(x => x.ItemTypes)
           .Returns(_marketContext.ItemTypes);

        context.SetupGet(x => x.PropertyGroups)
            .Returns(_marketContext.PropertyGroups);

        context.SetupGet(x => x.ItemProperties)
            .Returns(_marketContext.ItemProperties);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var group = TestHelper.GetOrdinaryPropertyGroup();

        var properties = new ItemProperty[]
        {
            TestHelper.GetOrdinaryItemProperty(1, "PropName1", "Value1", group),
            TestHelper.GetOrdinaryItemProperty(2, "PropName1", "ChangableValue", group),
            TestHelper.GetOrdinaryItemProperty(3, "PropName1", "Value3", group)
        };

        var type = TestHelper.GetOrdinaryItemType();

        await AddPropertyGroupAsync(group);

        await AddItemTypeAsync(type);

        await Task.WhenAll(properties.Select(async x => AddItemPropertyAsync(x)).ToArray());

        var inputItem = TestHelper.GetOrdinaryItem(1, type: type, properties: properties.Take(2).ToArray());

        var expectedItem = new TItem[]
        {
            TestHelper.GetStorageItem(inputItem)
        }
        .Select(x => { x.Description = null!; x.Type = null!; return x; })
        .ToArray();

        var expectedDescription = properties
            .Select(x => TestHelper.GetStorageItemPropertyWithOutItem(x))
            .Select(x => { x.ItemId = inputItem.Key.Value; x.Property = null!; return x; })
            .ToArray();

        await AddItemAsync(inputItem);

        var repository = new ProductsRepository(
            context.Object,
            logger);

        properties[0] = TestHelper.GetOrdinaryItemProperty(1, "NewPropName", "Value1", group);
        properties[1] = TestHelper.GetOrdinaryItemProperty(2, "PropName1", "NewValue", group);

        var updatedItem = TestHelper.GetOrdinaryItem(1, type, "NewName", properties.Take(2).ToArray());

        // Act
        var exception = Record.Exception(() =>
        {
            repository.Update(updatedItem);
            repository.Save();
        });
        var newItem = repository.GetByKey(updatedItem.Key);

        // Assert
        exception.Should().BeNull();
        newItem.Should().BeEquivalentTo(updatedItem);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can add product.")]
    [Trait("Category", "Integration")]
    public async Task CanAddProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        var provider1 = TestHelper.GetOrdinaryProvider();

        var type1 = TestHelper.GetOrdinaryItemType();

        var item1 = TestHelper.GetOrdinaryItem(type: type1, properties: Array.Empty<ItemProperty>());

        await AddProviderAsync(provider1);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);

        var inputProduct = TestHelper.GetOrdinaryProduct(item1, provider1);

        var expectedProduct = new TProduct[]
        {
                TestHelper.GetStorageProduct(inputProduct)
        }
        .Select(x => { x.Provider = null!; x.Item = null!; return x; })
        .ToArray();

        var repository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        using var transaction = _marketContext.Database
            .BeginTransaction();

        await repository.AddOrUpdateAsync(inputProduct)
            .ConfigureAwait(false);

        await _marketContext.SaveChangesAsync()
            .ConfigureAwait(false);

        await transaction.CommitAsync()
            .ConfigureAwait(false);

        var dbProducts = await GetTableRecordsAsync(
            "products",
            r => new TProduct
            {
                ItemId = r.GetInt64(0),
                ProviderId = r.GetInt64(1),
                ProviderCost = r.GetDecimal(2),
                Quantity = r.GetInt32(3)
            });

        // Assert
        dbProducts.Should().BeEquivalentTo(expectedProduct);
    }

    [Theory(DisplayName = $"The {nameof(ProductsRepository)} can contains works.")]
    [Trait("Category", "Integration")]
    [MemberData(nameof(ContainsDataProduct))]
    public async Task CanContainsProductWorksAsync(Product inputProduct, bool expectedResult)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        var provider1 = TestHelper.GetOrdinaryProvider(1, info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
            inn: "1111111111",
            acc: "11111111111111111111"));

        var provider2 = TestHelper.GetOrdinaryProvider(2, info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
            inn: "2111111112",
            acc: "21111111111111111112"));

        var type1 = TestHelper.GetOrdinaryItemType();

        var item1 = TestHelper.GetOrdinaryItem(1, type1, "Item1", properties: Array.Empty<ItemProperty>());

        var item2 = TestHelper.GetOrdinaryItem(2, type1, "Item2", properties: Array.Empty<ItemProperty>());

        await AddProviderAsync(provider1);
        await AddProviderAsync(provider2);

        await AddItemTypeAsync(type1);

        await AddItemAsync(item1);
        await AddItemAsync(item2);

        await AddProductAsync(TestHelper.GetOrdinaryProduct(item1, provider1));
        await AddProductAsync(TestHelper.GetOrdinaryProduct(item2, provider1));
        await AddProductAsync(TestHelper.GetOrdinaryProduct(item2, provider2));

        var repository = new ProductsRepository(
            context.Object,
            logger);

        // Act
        var result = await repository.ContainsAsync(inputProduct)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can update product.")]
    [Trait("Category", "Integration")]
    public async Task CanUpdateProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);

        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var provider = TestHelper.GetOrdinaryProvider();

        var type = TestHelper.GetOrdinaryItemType();

        var item = TestHelper.GetOrdinaryItem(1, type, "Name1", properties: Array.Empty<ItemProperty>());

        var oldProduct = TestHelper.GetOrdinaryProduct(item, provider, price: 100m, quantity: 2);

        await AddProviderAsync(provider);

        await AddItemTypeAsync(type);

        await AddItemAsync(item);

        await AddProductAsync(oldProduct);

        var repository = new ProductsRepository(
            context.Object,
            logger);

        var updatedProduct = TestHelper.GetOrdinaryProduct(item, provider, price: 400m, quantity: 5);

        // Act
        await repository.AddOrUpdateAsync(updatedProduct);
        repository.Save();

        var result = repository.GetByKey((item.Key, provider.Key));

        // Assert
        result.Should().BeEquivalentTo(updatedProduct).And.NotBeNull();
        result!.Quantity.Should().Be(updatedProduct.Quantity);
        result!.ProviderCost.Should().Be(updatedProduct.ProviderCost);
    }

    [Fact(DisplayName = $"The {nameof(ProductsRepository)} can delete product by entity and by key.")]
    [Trait("Category", "Integration")]
    public async Task CanRemoveProductAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductsRepository>>();

        var context = new Mock<IRepositoryContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Products)
            .Returns(_marketContext.Products);
        context.Setup(x => x.SaveChanges())
            .Callback(() => _marketContext.SaveChanges());

        var provider = TestHelper.GetOrdinaryProvider();

        var type = TestHelper.GetOrdinaryItemType();

        var item1 = TestHelper.GetOrdinaryItem(1, type, "Name1", properties: Array.Empty<ItemProperty>());

        var item2 = TestHelper.GetOrdinaryItem(2, type, "Name2", properties: Array.Empty<ItemProperty>());

        await AddProviderAsync(provider);

        await AddItemTypeAsync(type);

        await AddItemAsync(item1);
        await AddItemAsync(item2);

        await AddProductAsync(TestHelper.GetOrdinaryProduct(item1, provider));
        await AddProductAsync(TestHelper.GetOrdinaryProduct(item2, provider));

        var repository = new ProductsRepository(
            context.Object,
            logger);

        var inputProduct = TestHelper.GetOrdinaryProduct(item1, provider);

        // Act
        var beforeContains1 = await repository.ContainsAsync(inputProduct)
            .ConfigureAwait(false);

        var exception = Record.Exception((Action)(() =>
        {
            repository.Delete(inputProduct);
            repository.Save();
        }));

        var afterContains1 = await repository.ContainsAsync(inputProduct)
            .ConfigureAwait(false);

        // Assert
        exception.Should().BeNull();
        beforeContains1.Should().BeTrue();
        afterContains1.Should().BeFalse();
    }

    private async Task AddProviderAsync(Provider provider)
    {
        var fromQuery = "providers (id, name, margin, bank_account, inn)";
        var valuesQuery =
            $"({provider.Key.Value}, " +
            $"'{provider.Name}', " +
            $"{provider.Margin.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"'{provider.PaymentTransactionsInformation.BankAccount}', " +
            $"'{provider.PaymentTransactionsInformation.INN}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddItemAsync(Item item)
    {
        var fromQuery = "items (id, name, type_id)";
        var valuesQuery = $"({item.Key.Value}, '{item.Name}', {item.Type.Id})";

        await AddAsync(fromQuery, valuesQuery);

        await Task.WhenAll(item.Properties.Select(x => { AddItemDescriptionAsync(item.Key, x).Wait(); return Task.CompletedTask; }).ToArray());
    }

    private async Task AddItemDescriptionAsync(ID itemId, ItemProperty property)
    {
        var fromPropertyQuery = "item_description (item_id, property_id, property_value)";
        var valuesQuery = $"({itemId.Value}, {property.Key.Value}, '{property.Value}')";

        await AddAsync(fromPropertyQuery, valuesQuery);
    }

    private async Task AddItemTypeAsync(ItemType type)
    {
        var fromQuery = "item_type (id, name)";
        var valuesQuery = $"({type.Id}, '{type.Name}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddItemPropertyAsync(ItemProperty property)
    {
        var fromPropertyQuery = "item_properties (id, group_id, is_filterable, name, data_type_id)";
        var valuesQuery = $"({property.Key.Value}, {property.Group.Id.Value}, {property.IsFilterable}, '{property.Name}', {(int)property.ProperyDataType})";

        await AddAsync(fromPropertyQuery, valuesQuery);
    }

    private async Task AddPropertyGroupAsync(PropertyGroup group)
    {
        var fromQuery = "property_group (id, name)";
        var valuesQuery = $"({group.Id.Value}, '{group.Name}')";

        await AddAsync(fromQuery, valuesQuery);
    }

    private async Task AddProductAsync(Product product)
    {
        var fromQuery = "products (item_id, provider_cost, quantity, provider_id)";
        var valuesQuery =
            $"({product.Item.Key.Value}, " +
            $"{product.ProviderCost.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"{product.Quantity}, " +
            $"{product.Provider.Key.Value})";

        await AddAsync(fromQuery, valuesQuery);
    }


    public static readonly TheoryData<Item, bool> ContainsData = new()
    {
        {
            TestHelper.GetOrdinaryItem(
                1,
                TestHelper.GetOrdinaryItemType(),
                "Name1"),
            true
        },
        {
            TestHelper.GetOrdinaryItem(
                4,
                TestHelper.GetOrdinaryItemType(),
                "Name4"),
            false
        },
    };

    public static readonly TheoryData<Product, bool> ContainsDataProduct = new()
    {
        {
            TestHelper.GetOrdinaryProduct(
                TestHelper.GetOrdinaryItem(1),
                TestHelper.GetOrdinaryProvider(1,
                    info: TestHelper.GetOrdinaryPaymentTransactionsInformation(inn: "1234512345", acc: "12345123451234512345"))),
            true
        },
        {
            TestHelper.GetOrdinaryProduct(
                TestHelper.GetOrdinaryItem(4),
                TestHelper.GetOrdinaryProvider(4,
                    info: TestHelper.GetOrdinaryPaymentTransactionsInformation(inn: "5234512345", acc: "52345123451234512345"))),
            false
        },
    };
}
