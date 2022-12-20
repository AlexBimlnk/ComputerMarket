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

    public void ProviderArpove(Order order, Provider provider, bool value);

    public IEnumerable<Order> GetAproveOrdersOnProvider(Provider provider);
}
