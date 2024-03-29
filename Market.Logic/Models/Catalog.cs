﻿using Market.Logic.Models.Abstractions;
using Market.Logic.Storage.Repositories;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Каталог продуктов.
/// </summary>
public sealed class Catalog : ICatalog
{
    private readonly IProductsRepository _productRepository;
    private readonly IItemsRepository _itemRepository;

    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="Catalog"/>.
    /// </summary>
    /// <param name="productRepository">Репозиторий продуктов.</param>
    /// <param name="itemRepository">Репозиторий товаров.</param>
    /// <exception cref="ArgumentNullException">Если один из параметров -  <see langword="null"/>.</exception>
    public Catalog(IProductsRepository productRepository, IItemsRepository itemRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    /// <inheritdoc/>
    public IEnumerable<ItemType> GetItemTypes() =>
        _itemRepository
        .GetEntities()
        .ToList()
        .Select(x => x.Type)
        .DistinctBy(x => x.Id);

    /// <inheritdoc/>
    public IEnumerable<Product> GetProducts(ICatalogFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var products = _productRepository.GetEntities();

        if (filter.SearchString is not null)
        {
            products = products.Where(x => x.Item.Name.Contains(filter.SearchString, StringComparison.CurrentCultureIgnoreCase));
        }

        if (filter.SelectedTypeId is not null)
        {
            products = products.Where(x => x.Item.Type.Id == filter.SelectedTypeId);
        }

        foreach(var filterValue in filter.PropertiesWithValues)
        {
            products = products
            .Where(x => x.Item.Properties.Where(prop =>
                prop.Value is not null && prop.Key == filterValue.Item1 && prop.Value == filterValue.Item2).Any());
        }

        return products;
    }

    /// <inheritdoc/>
    public Product? GetProductByKey((ID, ID) key) => _productRepository.GetByKey(key);
    
    /// <inheritdoc/>
    public IEnumerable<Product> GetProducts() => _productRepository.GetEntities();

    /// <inheritdoc/>
    public IEnumerable<Item> GetItems() => _itemRepository.GetEntities();
}
