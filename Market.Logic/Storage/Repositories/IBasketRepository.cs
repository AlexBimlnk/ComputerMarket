using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;

/// <summary xml:lang="ru">
/// Репоиторий для продуктов пользователя в корзине.
/// </summary>
public interface IBasketRepository
{
    /// <summary xml:lang="ru">
    /// Добавление продукта в корзину.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользоватлеь корзины.</param>
    /// <param name="product" xml:lang="ru">Продут, который бдует добавлен.</param>
    public Task AddToBasketAsync(User user, Product product, CancellationToken token = default);

    /// <summary xml:lang="ru">
    /// Уменьшение колличества продуктов в корзине.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <param name="product" xml:lang="ru">Продукт.</param>
    public void RemoveFromBasket(User user, Product product);

    /// <summary>
    ///  Удаляет продукт из корзины пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="product">Продукт который должен быть удалён.</param>
    public void DeleteFromBasket(User user, Product product);

    /// <summary xml:lang="ru">
    /// Получение коллекции продуктов в корзине.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <returns xml:lang="ru">Продукты в корзине пользователя.</returns>
    public IEnumerable<PurchasableEntity> GetAllBasketItems(User user);

    /// <summary xml:lang = "ru">
    /// Сохраняет все накопленные команды.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public void Save();
}
