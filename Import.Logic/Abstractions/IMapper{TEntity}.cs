using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Models;

namespace Import.Logic.Abstractions;
/// <summary>
/// Интерфейс маппера
/// </summary>
/// <typeparam name="TEntity">
/// Сущность которая будет маппаться
/// </typeparam>
public interface IMapper<TEntity> where TEntity : IMappableEntity<InternalID, ExternalID>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TEntity MapEntity(TEntity entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public IReadOnlyCollection<TEntity> MapCollection(IReadOnlyCollection<TEntity> entities);

}
