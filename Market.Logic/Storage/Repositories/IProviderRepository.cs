using General.Storage;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;

namespace Market.Logic.Storage.Repositories;

public interface IProvidersRepository : IKeyableRepository<Provider, ID>
{
    /// <summary xml:lang="ru">
    /// Обновление информации о поставщике.
    /// </summary>
    /// <param name="provider" xml:lang="ru">Поставщик.</param>
    public void Update(Provider provider);

    /// <summary>
    /// Возвращает списко всех пользователей являющихся представителями.
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public IEnumerable<User> GetAgents(Provider provider);

    /// <summary>
    /// Добавляет нового прдеставителя.
    /// </summary>
    /// <param name="agent">Представитель.</param>
    public void AddAgent(ProviderAgent agent);

    /// <summary>
    /// Удаляет представителя.
    /// </summary>
    /// <param name="agent"></param>
    public void RemoveAgent(ProviderAgent agent);

    /// <summary>
    /// Возвращает текущего представителя которым является пользователь.
    /// </summary>
    /// <param name="user">Пользователь по которому ищется агент.</param>
    /// <returns>Агент от пользователя.</returns>
    public ProviderAgent GetAgent(User user);
}
