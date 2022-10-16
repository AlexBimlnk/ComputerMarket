using General.Storage;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Маппер продуктов.
/// </summary>
public sealed class Mapper : IMapper<Product>
{
    private readonly IKeyableCache<Link, ExternalID> _cache;
    private readonly ILogger<Mapper> _logger;
    private readonly IHistoryRecorder _historyRecorder;

    /// <summary xml:lang = "ru">
    /// Создание нового экземпляра типа <see cref="Mapper"/>.
    /// </summary>
    /// <param name="cache" xml:lang = "ru">Кэш с ссылками для маппинга.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <param name="historyRecorder" xml:lang = "ru">
    /// Записывателей истории неспамленных продуктов.
    /// </param>
    public Mapper(
        IKeyableCache<Link, ExternalID> cache,
        ILogger<Mapper> logger,
        IHistoryRecorder historyRecorder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _historyRecorder = historyRecorder ?? throw new ArgumentNullException(nameof(historyRecorder));
    }

    /// <inheritdoc/>
    public async ValueTask<Product> MapEntityAsync(Product entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        token.ThrowIfCancellationRequested();

        var linkOfEntity = _cache.GetByKey(entity.ExternalID);

        if (linkOfEntity is null)
        {
            _logger.LogWarning("Product with {ExternalID} doesn't mapped.", entity.ExternalID);
            await _historyRecorder.RecordHistoryAsync(entity);
            return entity;
        }

        entity.MapTo(linkOfEntity.InternalID);
        _logger.LogDebug("Product with {ExternalID} succesfully mapped to {InternalID}", entity.InternalID, entity.InternalID);

        return entity;
    }

    /// <inheritdoc/>
    public async ValueTask<IReadOnlyCollection<Product>> MapCollectionAsync(
        IReadOnlyCollection<Product> entityCollection,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entityCollection);

        token.ThrowIfCancellationRequested();

        var products = new List<Product>(entityCollection.Count);

        foreach (var product in entityCollection)
        {
            products.Add(await MapEntityAsync(product, token));
        }

        return products
            .Where(x => x.IsMapped)
            .ToList();
    }
}
