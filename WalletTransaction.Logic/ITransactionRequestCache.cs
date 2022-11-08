using General.Storage;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Описывкт кэш запросов на проведение транзакций.
/// </summary>
public interface ITransactionRequestCache : IKeyableCache<TransactionRequest, InternalID>
{
    /// <summary xml:lang = "ru">
    /// Отменяет запрос.
    /// </summary>
    /// <param name="requestId" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    public void CancelRequest(InternalID requestId);

    /// <summary xml:lang = "ru">
    /// Возвращает токен отмены для запроса.
    /// </summary>
    /// <param name="requestId" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    public CancellationToken GetCancellationTokenByRequest(InternalID requestId);
}
