﻿using System.Collections.Concurrent;

using General.Storage;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Кэш запросов на проведение транзакций.
/// </summary>
public sealed class TransactionsRequestCache : IKeyableCache<TransactionRequest, InternalID>
{
    private readonly ConcurrentDictionary<InternalID, TransactionRequest> _requests = new();

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public void Add(TransactionRequest entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (!_requests.TryAdd(entity.Key, entity))
        {
            throw new InvalidOperationException($"Attempt to add exsisting {nameof(TransactionRequest)} to collection");
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entities"/> - <see langword="null"/>.
    /// </exception>
    public void AddRange(IEnumerable<TransactionRequest> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        foreach (var entity in entities)
        {
            Add(entity);
        }
    }

    /// <inheritdoc/>
    public bool Contains(InternalID key) => _requests.ContainsKey(key);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public bool Contains(TransactionRequest entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return _requests.ContainsKey(entity.Key);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public void Delete(TransactionRequest entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (!_requests.TryRemove(entity.Key, out _))
        {
            throw new InvalidOperationException($"Attempt to delete not exsisting {nameof(TransactionRequest)}");
        }
    }

    /// <inheritdoc/>
    public TransactionRequest? GetByKey(InternalID key)
    {
        _ = _requests.TryGetValue(key, out var request);

        return request;
    }
}
