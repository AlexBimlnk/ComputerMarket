using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Models;

namespace Import.Logic.Abstractions;
/// <summary>
/// Описывает маппер для соедения внещних сущностей с внтуреними.
/// </summary>
/// <typeparam name="TEntity">
/// Сущность которая будет маппиться.
/// </typeparam>
public interface IMapper<TEntity> where TEntity : IMappableEntity<InternalID, ExternalID>
{
    /// <summary>
    /// Связывание сущности по ссылке.
    /// </summary>
    /// <param name="entity">Сущность для маппинга.</param>
    /// <returns>Результат маппинга сущности.</returns>
    public TEntity MapEntity(TEntity entity);

    /// <summary>
    /// Маппинг коллекции сущностей.
    /// </summary>
    /// <param name="entities">Коллекция сущостей.</param>
    /// <returns>Коллекция сущностей после маппинга.</returns>
    public IReadOnlyCollection<TEntity> MapCollection(IReadOnlyCollection<TEntity> entities);

}
