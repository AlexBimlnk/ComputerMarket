namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Каталог твоаров.
/// </summary>
public interface ICatalog
{
    /// <summary xml:lang="ru">
    /// Возвращает все типы товаров.
    /// </summary>
    /// <returns>Коллекция типов товаров.</returns>
    public IEnumerable<ItemType> GetItemTypes();

    /// <summary xml:lang="ru">
    /// Получение продуктов по заданому фильтру.
    /// </summary>
    /// <param name="filter">Фильтр для продутов.</param>
    /// <returns>Коллекция продутов удовлятворяющих условиям фильтра.</returns>
    public IEnumerable<Product> GetProducts(ICatalogFilter filter);

    /// <summary xml:lang="ru">
    /// Возращает список свойств с их значениями выбранной колекции продктов.
    /// </summary>
    /// <param name="products" xml:lang="ru">Входная колекция продуктов.</param>
    /// <returns xml:lang="ru">Коллеккция свойств с их значениями из колекции продуктов.</returns>
    public static IReadOnlyDictionary<ID, IFilterProperty> GetProductsProperties(IEnumerable<Product> products)
    {
        var result = new Dictionary<ID, IFilterProperty>();

        products
            .SelectMany(x => x.Item.Properties)
            .Where(x => x.Value is not null)
            .ToList()
            .ForEach(x =>
            {
                var property = new FilterProperty(x);
                var value = new FilterValue(x.Value!);

                if (!result.ContainsKey(x.Key))
                {
                    result.Add(x.Key, property);
                }

                result[x.Key].AddValue(value);
            });

        return result;
    }
}
