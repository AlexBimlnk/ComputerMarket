﻿using General.Storage;

using Import.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Import.Logic.Storage.Repositories;

using TLink = Models.Link;

/// <summary xml:lang = "ru">
/// Репозиторий связей.
/// </summary>
public sealed class LinkRepository : IRepository<Link>
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<LinkRepository> _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="LinkRepository"/>.
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
    public LinkRepository(
        IRepositoryContext context,
        ILogger<LinkRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private static TLink ConvertToStorageModel(Link link) => new()
    {
        InternalId = link.InternalID.Value,
        ExternalId = link.ExternalID.Value,
        ProviderId = (short)link.ExternalID.Provider,
    };

    private Link? ConvertFromStorageModel(TLink link)
    {
        if (!Enum.IsDefined(typeof(Provider), (int)link.ProviderId))
        {
            _logger.LogWarning(
                "The link with provider id: {ProviderId} can't be converted",
                link.ProviderId);

            return null;
        }

        return new(
            new(link.InternalId),
            new(link.ExternalId, (Provider)link.ProviderId));
    }

    /// <inheritdoc/>
    public async Task AddAsync(Link entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Links.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Link entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Links.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(Link entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Links.Remove(ConvertToStorageModel(entity));
    }

    /// <inheritdoc/>
    public IEnumerable<Link> GetEntities() =>
        _context.Links
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x))
        .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();
}
