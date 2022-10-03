using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Import.Logic;
/// <summary>
/// Маппер продуктов.
/// </summary>
public class Mapper: IMapper<Product>
{
    private readonly IKeyableCache<Link, ExternalID> _cache;
    private readonly ILogger<Mapper> _logger;
    /// <summary>
    /// Создание нового экземпляра типа <see cref="Mapper"/>.
    /// </summary>
    /// <param name="cache">кэш с ссылками для маппинга</param>
    /// <param name="logger">логгер</param>
    /// 
    public Mapper(IKeyableCache<Link, ExternalID> cache, ILogger<Mapper> logger)
    {
        ArgumentNullException.ThrowIfNull(cache, nameof(cache));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _logger = logger;
        _cache = cache;
    }

    /// <inheritdoc/>
    public Product MapEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var linkOfEntity = _cache.GetByKey(entity.ExternalID);

        if (linkOfEntity is null)
            return entity;

        entity.MapTo(linkOfEntity.InternalID);

        return entity;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Product> MapCollection(IReadOnlyCollection<Product> entityCollection) => 
        entityCollection.Select((x) => MapEntity(x)).Where(x => x.IsMapped).ToList();
}
