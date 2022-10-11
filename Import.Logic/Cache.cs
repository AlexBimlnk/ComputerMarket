using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Import.Logic;
public sealed class Cache : IKeyableCache<Link, ExternalID>
{
    private readonly Dictionary<ExternalID, Link> _dictionaryCache;

    public Cache()
    {
        _dictionaryCache = new Dictionary<ExternalID, Link>();
    }

    public void Add(Link entity)
    {
        if (!_dictionaryCache.ContainsKey(entity.ExternalID))
        {
            _dictionaryCache[entity.ExternalID] = entity;
        }
    }

    public void AddRange(IEnumerable<Link> entities)
    {
        foreach(var entity in entities)
        {
            Add(entity);
        }
    }

    public bool Contains(ExternalID key) => _dictionaryCache.ContainsKey(key);

    public bool Contains(Link entity) => _dictionaryCache.ContainsKey(entity.ExternalID);
    public void Delete(Link entity) => _dictionaryCache.Remove(entity.ExternalID);
    public Link? GetByKey(ExternalID key)
    {
        Link? returnable;

        _ = _dictionaryCache.TryGetValue(key, out returnable);

        return returnable;
    }
}
