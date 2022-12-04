using General.Storage;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

using ItemDescription = Models.ItemDescription;
using TItem = Models.Item;
using TItemProperty = Models.ItemProperty;
using TProduct = Models.Product;
using TPropertyGroup = Models.PropertyGroup;
using TProvider = Models.Provider;

public sealed class ProductsRepository : IItemsRepository, IProductsRepository
{
    private readonly ILogger<ProductsRepository> _logger;
    private readonly IRepositoryContext _context;

    public ProductsRepository(IRepositoryContext context, ILogger<ProductsRepository> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task AddAsync(Item entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        var newItem = ConvertToStorageModel(entity);

        if (_context.ItemTypes.Contains(newItem.Type))
        {
            newItem.Type = default!;
        }

        newItem.Description = newItem.Description.Select(x =>
        {
            if (_context.PropertyGroups.Contains(x.Property.Group))
            {
                x.Property.Group = default!;
            }

            if (_context.ItemProperties.Contains(x.Property))
            {
                x.Property = default!;
            }

            return x;
        }).ToArray();

        await _context.Items.AddAsync(newItem, token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Item entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Items.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(Item entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var storageItem = ConvertToStorageModel(entity);

        storageItem.Description = storageItem.Description.Select(x =>
        {
            x.Property = default!;

            return x;
        }).ToArray();

        storageItem.Type = default!;

        _context.Items.Remove(storageItem);
    }

    /// <inheritdoc/>
    public Item? GetByKey(ID key) =>
        _context.Items
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .SingleOrDefault();

    /// <inheritdoc/>
    public void Update(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var storageItem = ConvertToStorageModel(item);

        var groupTrackCheck = new HashSet<int>();

        if (!_context.ItemTypes.Contains(storageItem.Type))
        {
            _context.ItemTypes.Add(storageItem.Type);
        }

        storageItem.Description = storageItem.Description
            .Select(x =>
            {
                if (x.Property.Group is not null &&
                    !groupTrackCheck.Contains(x.Property.Group.Id))
                {
                    if (!_context.PropertyGroups.Contains(x.Property.Group))
                    {
                        _context.PropertyGroups.Add(x.Property.Group);
                    }

                    groupTrackCheck.Add(x.Property.Group.Id);
                }

                if (x.Property.Group is not null)
                    x.Property.Group = _context.PropertyGroups.Single(xx => xx.Id == x.Property.Group!.Id);

                if (!_context.ItemProperties.Contains(x.Property))
                {
                    _context.ItemProperties.Add(x.Property);
                }

                return x;
            })
            .ToArray();

        _context.Items.Update(storageItem);
    }

    /// <inheritdoc/>
    IEnumerable<Item> IRepository<Item>.GetEntities() =>
        _context.Items
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x));

    public async Task AddAsync(Product entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _context.Products.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateAsync(Product entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        var product = await _context.Products
            .FindAsync(new object[] { entity.Key.Item2.Value, entity.Key.Item1.Value }, token)
            .ConfigureAwait(false);

        if (product is null)
        {
            await _context.Products.AddAsync(ConvertToStorageModel(entity), token)
                .ConfigureAwait(false);

            return;
        }

        product.ProviderCost = entity.ProviderCost;
        product.Quantity = entity.Quantity;

        _context.Products.Update(product);
    }

    /// <inheritdoc/>
    public void Delete(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var storage = ConvertToStorageModel(entity);

        _context.Products.Remove(storage);
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        await Task.WhenAll(products.Select(async x => await AddOrUpdateAsync(x)));
    }

    /// <inheritdoc/>
    public Product? GetByKey((ID, ID) key)
    {
        var product = _context.Products
            .Find(key.Item1.Value, key.Item2.Value);

        return product is null ? null : ConvertFromStorageModel(product);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Product entity, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return await _context.Products
            .AsNoTrackingWithIdentityResolution()
            .ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IEnumerable<Product> GetEntities() =>
        _context.Products
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x));

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    #region Converters

    private static TItem ConvertToStorageModel(Item item)
    {
        var tItem = new TItem()
        {
            Id = item.Key.Value,
            Name = item.Name,
            TypeId = item.Type.Id,
            Type = new Models.ItemType()
            {
                Id = item.Type.Id,
                Name = item.Type.Name
            }
        };

        tItem.Description = item.Properties
           .Select(x => ConvertToStorageModel(x))
           .Select(x => { x.Item = tItem; x.ItemId = tItem.Id; return x; })
           .ToArray();

        return tItem;
    }

    private static Item ConvertFromStorageModel(TItem item) => new(
        new ID(item.Id),
        new ItemType(item.TypeId, item.Type.Name),
        item.Name,
        item.Description
            .Select(x => ConvertFromStorageModel(x))
            .ToArray());

    private static ItemDescription ConvertToStorageModel(ItemProperty property)
    {
        var tProperty = new TItemProperty()
        {
            Id = property.Key.Value,
            Name = property.Name,
            GroupId = (int)property.Group.Id.Value,
            Group = new TPropertyGroup()
            {
                Id = (int)property.Group.Id.Value,
                Name = property.Group.Name
            },
            IsFilterable = property.IsFilterable,
            PropertyDataTypeId = property.ProperyDataType
        };

        if (tProperty.GroupId == -1)
        {
            tProperty.GroupId = default;
            tProperty.Group = default;
        }

        return new ItemDescription()
        {
            PropertyId = tProperty.Id,
            Property = tProperty,
            PropertyValue = property.Value
        };
    }

    private static ItemProperty ConvertFromStorageModel(ItemDescription property)
    {
        var group = property.Property.GroupId is not null
                ? new PropertyGroup(new ID(property.Property.GroupId.GetValueOrDefault()), property.Property.Group!.Name)
                : PropertyGroup.Default;

        var dProperty = new ItemProperty(
            new ID(property.PropertyId),
            property.Property.Name,
            group,
            property.Property.IsFilterable,
            (PropertyDataType)property.Property.PropertyDataTypeId);

        if (property.PropertyValue is not null)
        {
            dProperty.Value = property.PropertyValue;
        }

        return dProperty;
    }

    private static TProduct ConvertToStorageModel(Product product) => new()
    {
        ProviderCost = product.ProviderCost,
        Quantity = product.Quantity,
        ItemId = product.Item.Key.Value,
        ProviderId = product.Provider.Key.Value,
    };

    private static Product ConvertFromStorageModel(TProduct product)
      => new Product(
          ConvertFromStorageModel(product.Item),
          ConvertFromStorageModel(product.Provider),
          new Price(product.ProviderCost),
          product.Quantity);

    private static TProvider ConvertToStorageModel(Provider provider) => new()
    {
        Id = provider.Key.Value,
        Name = provider.Name,
        Margin = provider.Margin.Value,
        Inn = provider.PaymentTransactionsInformation.INN,
        BankAccount = provider.PaymentTransactionsInformation.BankAccount
    };

    private static Provider ConvertFromStorageModel(TProvider provider) =>
       new Provider(
           new ID(provider.Id),
           provider.Name,
           new Margin(provider.Margin),
           new PaymentTransactionsInformation(provider.Inn, provider.BankAccount));

    #endregion
}
