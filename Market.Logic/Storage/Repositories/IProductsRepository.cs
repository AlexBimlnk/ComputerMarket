using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;
public interface IProductsRepository : IKeyableRepository<Product, (ID, ID)>
{
    /// <summary xml:lang = "ru">
    /// Добавлляет или обновляет продукт.
    /// </summary>
    /// <param name="product" xml:lang = "ru">Продукт.</param>
    /// <param name="token" xml:lang = "ru"></param>
    /// <returns xml:lang = "ru"></returns>
    public Task AddOrUpdateAsync(Product product, CancellationToken token = default);

    /// <summary xml:lang = "ru">
    /// Добавляет мнодство продуктов.
    /// </summary>
    /// <param name="products">Продукты.</param>
    /// <returns xml:lang = "ru"></returns>
    public Task AddRangeAsync(IEnumerable<Product> products);
}
