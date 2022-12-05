using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;

/// <summary xml:lang = "ru">
/// Интерфейс репозитория товаров.
/// </summary>
public interface IItemsRepository : IKeyableRepository<Item, ID>
{
    /// <summary xml:lang = "ru">
    /// Обновляет описание товара.
    /// </summary>
    /// <param name="item" xml:lang = "ru">Товар с обновленным описанием.</param>
    public void Update(Item item);
}
