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
    /// <param name="entity" xml:lang="ru">Продут, который бдует добавлен.</param>
    public void AddToBasket(User user, PurchasableEntity entity);

    /// <summary xml:lang="ru">
    /// Удаление из продукта корзины.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <param name="entity" xml:lang="ru">Продукт который будет удален из корзины.</param>
    public void RemoveFromBasket(User user, PurchasableEntity entity);

    /// <summary xml:lang="ru">
    /// Получение коллекции продуктво в корзине.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <returns xml:lang="ru">Продукты в корзине пользователя.</returns>
    public IEnumerable<PurchasableEntity> GetAllBasketItems(User user);
}
