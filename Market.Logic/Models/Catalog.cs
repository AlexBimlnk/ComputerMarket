using Market.Logic.Models.Abstractions;
using Market.Logic.Storage.Repositories;

using Microsoft.Extensions.Logging;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Каталог продуктов.
/// </summary>
public sealed class Catalog : ICatalog
{
    private readonly IProductsRepository _productRepository;
    private readonly IItemsRepository _itemRepository;
    private readonly ILogger<Catalog> _logger;

    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="Catalog"/>.
    /// </summary>
    /// <param name="productRepository">Репозиторий продуктов.</param>
    /// <param name="itemRepository">Репозиторий товаров.</param>
    /// <param name="logger">Логгер.</param>
    /// <exception cref="ArgumentNullException">Если один из параметров -  <see langword="null"/>.</exception>
    public Catalog(IProductsRepository productRepository, IItemsRepository itemRepository, ILogger<Catalog> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public IEnumerable<ItemType> GetItemTypes() =>
        _itemRepository
        .GetEntities()
        .Select(x => x.Type)
        .DistinctBy(x => x.Id)
        .AsEnumerable();

    /// <inheritdoc/>
    public IEnumerable<Product> GetProducts(ICatalogFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var products = _productRepository.GetEntities();

        if (filter.SearchString is not null)
        {
            var searchString = filter.SearchString;
            products = products.Where(x => x.Item.Name == searchString);
        }

        if (filter.SelectedType is not null)
        {
            var idOfType = filter.SelectedType.Id;

            filter.SelectedType = _itemRepository
                .GetEntities()
                .Select(x => x.Type)
                .Single(x => x.Id == idOfType);

            products = products.Where(x => x.Item.Type.Id == idOfType);
        }

        products = products
            .Where(x => x.Item.Properties.Where(prop =>
                prop.Value is not null &&
                filter.PropertiesWithValues.Contains((prop.Key, prop.Value)))
            .Any());

        return products.AsEnumerable();
    }

    /// <summary xml:lang="ru">
    /// Возращает список свойств с их значениями выбранной колекции продктов.
    /// </summary>
    /// <param name="products" xml:lang="ru">Входная колекция продуктов.</param>
    /// <returns xml:lang="ru">Коллеккция свойств с их значениями из колекции продуктов.</returns>
    public static Dictionary<ID, IFileterProperty> GetProductsProperties(IEnumerable<Product> products)
    {
        var result = new Dictionary<ID, IFileterProperty>();

        _ = products.SelectMany(x => x.Item.Properties)
            .Where(x => x.Value is not null)
            .Select(x =>
        {
            var property = new FilterProperty(x);
            var value = new FilterValue(x.Value!);

            if (!result.ContainsKey(x.Key))
            {
                result.Add(x.Key, property);
            }

            result[x.Key].AddValue(value);
            return x;
        }).ToList();

        return result;
    }
}
