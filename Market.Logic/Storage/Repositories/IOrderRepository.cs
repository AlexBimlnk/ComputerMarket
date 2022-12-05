using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;

/// <summary xml:lang="ru">
/// Репоиторий заказов.
/// </summary>
public interface IOrderRepository : IKeyableRepository<Order, ID>
{
    /// <summary xml:lang="ru">
    /// Обновлет состояние заказа.
    /// </summary>
    /// <param name="order" xml:lang="ru">Заказ с новым состоянием.</param>
    public void UpdateState(Order order);

    /// <summary xml:lang="ru">
    /// Добавление продукта в корзину.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользоватлеь корзины.</param>
    /// <param name="product" xml:lang="ru">Продут, который бдует добавлен.</param>
    public void AddToBasket(User user, Product product);

    /// <summary xml:lang="ru">
    /// Удаление из продукта корзины.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <param name="product" xml:lang="ru">Продукт который будет удален из корзины.</param>
    public void RemoveFromBasket(User user, Product product);

    /// <summary xml:lang="ru">
    /// Получение коллекции продуктво в корзине.
    /// </summary>
    /// <param name="user" xml:lang="ru">Пользователь корзины.</param>
    /// <returns xml:lang="ru">Продукты в корзине пользователя.</returns>
    public IEnumerable<PurchasableEntity> GetAllBasketItems(User user);
}
