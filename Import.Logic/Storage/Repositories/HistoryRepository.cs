using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Import.Logic.Storage.Repositories;

using THistory = Models.History;

/// <summary xml:lang = "ru">
/// Репозиторий связей.
/// </summary>
public sealed class HistoryRepository : IRepository<History>
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<HistoryRepository> _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="HistoryRepository"/>.
    /// </summary>
    /// <param name="context" xml:lang = "ru">
    /// Контекст репозиториев БД.
    /// </param>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров равен <see langword="null"/>.
    /// </exception>
    public HistoryRepository(
        IRepositoryContext context, 
        ILogger<HistoryRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private static THistory ConvertToStorageModel(History history) => new()
    {
        ExternalId = history.ExternalId.Value,
        ProviderId = (short)history.ExternalId.Provider,
        ProductMetadata = history.ProductMetadata
    };

    private History? ConvertFromStorageModel(THistory history)
    {
        if (!Enum.IsDefined(typeof(Provider), history.ProviderId))
        {
            _logger.LogWarning(
                "The link with provider id: {ProviderId} can't be converted",
                history.ProviderId);

            return null;
        }

        return new(
            new(history.ExternalId, (Provider)history.ProviderId),
            history.ProductMetadata);
    }

    /// <inheritdoc/>
    public async Task AddAsync(History entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Histories.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(History entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Histories.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(History entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Histories.Remove(ConvertToStorageModel(entity));
    }

    /// <inheritdoc/>
    public IEnumerable<History> GetEntities() =>
        _context.Histories
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x))
        .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();
}
