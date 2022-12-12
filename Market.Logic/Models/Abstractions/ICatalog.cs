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
}
