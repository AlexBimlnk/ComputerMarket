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
    //private readonly ILogger<Catalog> _logger;

    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="Catalog"/>.
    /// </summary>
    /// <param name="productRepository">Репозиторий продуктов.</param>
    /// <param name="itemRepository">Репозиторий товаров.</param>
    /// <param name="logger">Логгер.</param>
    /// <exception cref="ArgumentNullException">Если один из параметров -  <see langword="null"/>.</exception>
    public Catalog(IProductsRepository productRepository, IItemsRepository itemRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public IEnumerable<ItemType> GetItemTypes() =>
        _itemRepository
        .GetEntities()
        .Select(x => x.Type)
        .DistinctBy(x => x.Id);

    /// <inheritdoc/>
    public IEnumerable<Product> GetProducts(ICatalogFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var products = _productRepository.GetEntities();

        if (filter.SearchString is not null)
        {
            products = products.Where(x => x.Item.Name.Contains(filter.SearchString));
        }

        if (filter.SelectedTypeId is not null)
        {
            products = products.Where(x => x.Item.Type.Id == filter.SelectedTypeId);
        }

        if (filter.PropertiesWithValues.Any())
        {
            products = products
            .Where(x => x.Item.Properties.Where(prop =>
                prop.Value is not null &&
                filter.PropertiesWithValues.Contains((prop.Key, prop.Value)))
            .Any());
        }

        return products;
    }
}
