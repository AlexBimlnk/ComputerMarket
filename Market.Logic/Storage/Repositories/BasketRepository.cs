﻿using Market.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

using TBasketItem = Models.BasketItem;
using TItemDescription = Models.ItemDescription;
using TItem = Models.Item;
using TItemProperty = Models.ItemProperty;
using TProduct = Models.Product;
using TPropertyGroup = Models.PropertyGroup;
using TProvider = Models.Provider;

public sealed class BasketRepository : IBasketRepository
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<BasketRepository> _logger;

    public BasketRepository(IRepositoryContext context, ILogger<BasketRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddToBasketAsync(User user, Product product, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(product);

        token.ThrowIfCancellationRequested();

        await _context.BasketItems.AddAsync(ConvertToStorageModel(product, user), token)
            .ConfigureAwait(false);
    }

    public IEnumerable<PurchasableEntity> GetAllBasketItems(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return _context.BasketItems
            .Where(x => x.UserId == user.Key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .AsEnumerable();
    }

    public void RemoveFromBasket(User user, Product product)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(product);

        _context.BasketItems.Remove(ConvertToStorageModel(product, user));
    }

    #region Converter

    private static TBasketItem ConvertToStorageModel(Product product, User user) => new()
    {
        UserId = user.Key.Value,
        User = default!,
        Quantity = 1,
        ProviderId = product.Provider.Key.Value,
        ItemId = product.Item.Key.Value,
        Product = default!
    };

    private static PurchasableEntity ConvertFromStorageModel(TBasketItem basketItem) =>
        new PurchasableEntity(ConvertFromStorageModel(basketItem.Product), basketItem.Quantity);

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

    private static TItemDescription ConvertToStorageModel(ItemProperty property)
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

        return new TItemDescription()
        {
            PropertyId = tProperty.Id,
            Property = tProperty,
            PropertyValue = property.Value
        };
    }

    private static ItemProperty ConvertFromStorageModel(TItemDescription property)
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