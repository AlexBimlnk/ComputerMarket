using System.Collections.Concurrent;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Кэш запросов на проведение транзакций.
/// </summary>
public sealed class TransactionsRequestCache : ITransactionRequestCache
{
    private readonly ConcurrentDictionary<InternalID, (TransactionRequest, CancellationTokenSource)> _requests = new();

    private static CancellationTokenSource GenerateCTS() =>
        new CancellationTokenSource(TimeSpan.FromMinutes(5));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="entity"/> - <see langword="null"/>.
    /// </exception>
    public void Add(TransactionRequest entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (!_requests.TryAdd(entity.Key, (entity, GenerateCTS())))
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
    public void CancelRequest(InternalID requestId) =>
        _requests[requestId].Item2.Cancel();

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

        return request.Item1;
    }
}
