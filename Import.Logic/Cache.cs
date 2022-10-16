using System.Collections.Concurrent;

using General.Storage;

using Import.Logic.Models;

namespace Import.Logic;

/// <summary xml:lang = "ru">
///  Кэш связей.
/// </summary>
public sealed class Cache : IKeyableCache<Link, ExternalID>
{
    private readonly ConcurrentDictionary<ExternalID, Link> _dictionaryCache = new();

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public void Add(Link entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (!_dictionaryCache.TryAdd(entity.ExternalID, entity))
        {
            throw new InvalidOperationException($"Attempt to add exsisting {nameof(Link)} to collection");
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entities"/> - <see langword="null"/>.
    /// </exception>
    public void AddRange(IEnumerable<Link> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        foreach (var entity in entities)
        {
            Add(entity);
        }
    }

    /// <inheritdoc/>
    public bool Contains(ExternalID key) => _dictionaryCache.ContainsKey(key);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public bool Contains(Link entity) =>
        _dictionaryCache.ContainsKey((
            entity ?? throw new ArgumentNullException(
                nameof(entity),
                $"{nameof(entity)} is null")).ExternalID);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public void Delete(Link entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (!_dictionaryCache.TryRemove(entity.ExternalID, out var link))
        {
            throw new InvalidOperationException($"Attempt to delete not exsisting {nameof(Link)}");
        }
    }

    /// <inheritdoc/>
    public Link? GetByKey(ExternalID key)
    {
        _ = _dictionaryCache.TryGetValue(key, out var link);

        return link;
    }
}
