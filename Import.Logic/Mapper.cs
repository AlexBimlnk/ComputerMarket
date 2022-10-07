using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Маппер продуктов.
/// </summary>
public class Mapper : IMapper<Product>
{
    private readonly IKeyableCache<Link, ExternalID> _cache;
    private readonly ILogger<Mapper> _logger;

    /// <summary xml:lang = "ru">
    /// Создание нового экземпляра типа <see cref="Mapper"/>.
    /// </summary>
    /// <param name="cache" xml:lang = "ru">Кэш с ссылками для маппинга.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    public Mapper(IKeyableCache<Link, ExternalID> cache, ILogger<Mapper> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <inheritdoc/>
    public Product MapEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var linkOfEntity = _cache.GetByKey(entity.ExternalID);

        if (linkOfEntity is null)
        {
            _logger.LogWarning("Product with {ExternalID} doesn't mapped.", entity.ExternalID);
            return entity;
        }

        entity.MapTo(linkOfEntity.InternalID);
        _logger.LogDebug("Product with {ExternalID} succesfully mapped to {InternalID}", entity.InternalID, entity.InternalID);

        return entity;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Product> MapCollection(IReadOnlyCollection<Product> entityCollection) =>
        entityCollection.Select((x) => MapEntity(x)).Where(x => x.IsMapped).ToList();
}
