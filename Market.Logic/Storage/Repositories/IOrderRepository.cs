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
    /// Подвтерждение или отмена заказа поставщиком.
    /// </summary>
    /// <param name="order" xml:lang="ru">Заказ.</param>
    /// <param name="provider" xml:lang="ru">Поставщик.</param>
    /// <param name="value" xml:lang="ru">Ответ от поставщика.</param>
    public void ProviderArpove(Order order, Provider provider, bool value);

    /// <summary xml:lang="ru">
    /// Получение списка заказаов на поставщика.
    /// </summary>
    /// <param name="provider">Поставщик, на которого ищутся заказы.</param>
    /// <returns>Список заказаов на поставщика <paramref name="provider"/>.</returns>
    public IEnumerable<Order> GetProviderOrders(Provider provider);
}
