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
    /// Удаление из продукта корзины.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <param name="product" xml:lang="ru">Продукт который будет удален из корзины.</param>
    public void RemoveFromBasket(User user, Product product);

    /// <summary xml:lang="ru">
    /// Получение коллекции продуктов в корзине.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <returns xml:lang="ru">Продукты в корзине пользователя.</returns>
    public IEnumerable<PurchasableEntity> GetAllBasketItems(User user);
}
