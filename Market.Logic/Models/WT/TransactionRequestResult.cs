namespace Market.Logic.Models.WT;

/// <summary xml:lang = "ru">
/// Результат запроса на проведение транзакций.
/// </summary>
public sealed class TransactionRequestResult
{
    /// <summary xml:lang = "ru">
    /// Создает новый объект типа <see cref="TransactionRequestResult"/>.
    /// </summary>
    /// <param name="requestId" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    /// <param name="isCancelled" xml:lang = "ru">
    /// Флаг, указывающий на то, что запрос был отменён.
    /// </param>
    /// <param name="state" xml:lang = "ru">
    /// Состояние запроса.
    /// </param>
    public TransactionRequestResult(
        ID requestId,
        bool isCancelled,
        TransactionRequestState state)
    {
        TransactionRequestId = requestId;
        IsCancelled = isCancelled;
        State = state;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    public ID TransactionRequestId { get; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий на то, что запрос был отменён.
    /// </summary>
    public bool IsCancelled { get; }

    /// <summary xml:lang = "ru">
    /// Состояние запроса.
    /// </summary>
    public TransactionRequestState State { get; }
}
