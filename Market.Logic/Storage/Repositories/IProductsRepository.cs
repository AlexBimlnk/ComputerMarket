using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;

/// <summary xml:lang = "ru">
/// Интерфейс репозитория продутов.
/// </summary>
public interface IProductsRepository : IKeyableRepository<Item, ID>
{
    /// <summary xml:lang = "ru">
    /// Обновляет описание товара.
    /// </summary>
    /// <param name="item" xml:lang = "ru">Товар с обновленным описанием.</param>
    public void Update(Item item);

    /// <summary xml:lang = "ru">
    /// Добавлляет или обновляет продукт.
    /// </summary>
    /// <param name="product" xml:lang = "ru">Продукт.</param>
    /// <param name="token" xml:lang = "ru"></param>
    /// <returns xml:lang = "ru"></returns>
    public Task AddOrUpdateProductAsync(Product product, CancellationToken token = default);

    /// <summary xml:lang = "ru">
    /// Добавляет мнодство продуктов.
    /// </summary>
    /// <param name="products">Продукты.</param>
    /// <returns xml:lang = "ru"></returns>
    public Task AddProductRangeAsync(IReadOnlyCollection<Product> products);

    /// <summary xml:lang = "ru">
    /// Удалет продукт.
    /// </summary>
    /// <param name="product" xml:lang = "ru">Продукт, который нужно удлать.</param>
    public void RemoveProduct(Product product);

    /// <summary xml:lang = "ru">
    /// Удалеят продукт по ключу.
    /// </summary>
    /// <param name="key" xml:lang = "ru">Ключ продукта.</param>
    public void RemoveProduct((ID, ID) key);

    /// <summary xml:lang = "ru">
    /// Определяет наличие продукта по ключу.
    /// </summary>
    /// <param name="key" xml:lang = "ru">Ключ продукта.</param>
    /// <param name="token" xml:lang = "ru"></param>
    /// <returns xml:lang = "ru"><see langword="true"/> если - есть, <see langword="false"/> если - нет.</returns>
    public Task<bool> ContainsProductAsync((ID, ID) key, CancellationToken token = default);

    /// <summary xml:lang = "ru">
    /// Получает продукт по его ключу.
    /// </summary>
    /// <param name="key" xml:lang = "ru">Ключ продукта.</param>
    /// <returns xml:lang = "ru">Продут или <see langword="null"/>, если продукта с таким ключом нет.</returns>
    public Product? GetProduct((ID, ID) key);

    /// <summary xml:lang = "ru">
    /// Возвращает все продукты.
    /// </summary>
    /// <returns xml:lang = "ru">Все продукты.</returns>
    public IEnumerable<Product> GetAllProducts();

    /// <summary xml:lang = "ru">
    /// Возвращает продкуты соответвуюшего товара.
    /// </summary>
    /// <param name="item" xml:lang = "ru">Товар.</param>
    /// <returns xml:lang = "ru">Продукты товара.</returns>
    public IEnumerable<Product> GetAllItemProducts(Item item);

    /// <summary xml:lang = "ru">
    /// Возвращает продукты поставщика.
    /// </summary>
    /// <param name="provider" xml:lang = "ru">Поставщик.</param>
    /// <returns xml:lang = "ru">Продукты поставщика.</returns>
    public IEnumerable<Product> GetAllProviderProducts(Provider provider);
}
