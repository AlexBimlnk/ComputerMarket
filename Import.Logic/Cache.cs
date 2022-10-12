using System.Collections.Concurrent;

using Import.Logic.Abstractions;
using Import.Logic.Models;

namespace Import.Logic;

/// <summary xml:lang = "ru">
///  Кэш связей.
/// </summary>
public sealed class Cache : IKeyableCache<Link, ExternalID>
{
    private readonly ConcurrentDictionary<ExternalID, Link> _dictionaryCache;

    /// <summary xml:lang = "ru">
    /// Создание нового экземпляра типа <see cref="Cache"/>.
    /// </summary>
    public Cache()
    {
        _dictionaryCache = new ConcurrentDictionary<ExternalID, Link>();
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="entity"/> - <see langword="null"/>.</exception>
    public void Add(Link entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _dictionaryCache.TryAdd(entity.ExternalID, entity);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="entities"/> - <see langword="null"/>.</exception>
    public void AddRange(IEnumerable<Link> entities)
    {
        foreach (var entity in entities ?? throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} is null"))
        {
            Add(entity);
        }
    }

    /// <inheritdoc/>
    public bool Contains(ExternalID key) => _dictionaryCache.ContainsKey(key);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="entity"/> - <see langword="null"/>.</exception>
    public bool Contains(Link entity) =>
        _dictionaryCache.ContainsKey((entity ?? throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} is null")).ExternalID);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="entity"/> - <see langword="null"/>.</exception>
    public void Delete(Link entity)
    {
        Link link;
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _ = _dictionaryCache.TryRemove(entity.ExternalID, out link);
    }

    /// <inheritdoc/>
    public Link? GetByKey(ExternalID key)
    {
        Link? link;

        _ = _dictionaryCache.TryGetValue(key, out link);

        return link;
    }
}
