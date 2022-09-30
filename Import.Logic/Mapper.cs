using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Import.Logic;
public class Mapper: IMapper<Product>
{
    private readonly ICache<ExternalID ,Link> _cache;
    private readonly ILogger<Mapper> _logger;

    public Mapper(ICache<ExternalID, Link> cache, ILogger<Mapper> logger = null!)
    {
        ArgumentNullException.ThrowIfNull(cache, nameof(cache));
        
        _logger = logger;
        _cache = cache;
    }

    public Product MapEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        if (entity.IsMapped)
        {
            throw new ArgumentException("Entity already been mapped",nameof(entity));
        }

        if (!_cache.Contains(entity.ExternalID))
        {
            throw new ArgumentException($"Given entity doesn't exists in {nameof(_cache)}");
        }

        var linkOfEntity = _cache.GetByKey(entity.ExternalID);

        entity.MapTo(linkOfEntity.InternalID);

        _cache.Delete(linkOfEntity.ExternalID);

        return entity;
    }

    public IEnumerable<Product> MapEntityCollection(IEnumerable<Product> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));

        return entities.Select(e => MapEntity(e));
    }
}
