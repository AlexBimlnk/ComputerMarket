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
    /// Получение продуктов.
    /// </summary>
    /// <returns>Коллекция всех продутов.</returns>
    public IEnumerable<Product> GetProducts();

    /// <summary xml:lang="ru">
    /// Получение товаров.
    /// </summary>
    /// <returns>Коллекция всех товарво.</returns>
    public IEnumerable<Item> GetItems();

    /// <summary xml:lang="ru">
    /// Получение продука по его ключу.
    /// </summary>
    /// <param name="key" xml:lang="ru">Ключ продкта.</param>
    /// <returns xml:lang="ru">
    /// Продукт, который был найден с ключом <paramref name="key"/> или <see langword="null"/> если такого продукта - нет.
    /// </returns>
    public Product? GetProductByKey((ID, ID) key);
}
