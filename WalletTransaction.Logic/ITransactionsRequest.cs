namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Описывает запрос на проведение транзакций.
/// </summary>
public interface ITransactionsRequest
{
    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    public InternalID Id { get; }

    /// <summary xml:lang = "ru">
    /// Коллекция транзакций, которые нужно выполнить в рамках запроса.
    /// </summary>
    public IReadOnlyCollection<Transaction> Transactions { get; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий на то, что запрос был отменён.
    /// </summary>
    public bool IsCancelled { get; }

    /// <summary xml:lang = "ru">
    /// Текущие состояние запроса.
    /// </summary>
    public TransactionRequestState CurrentState { get; set; }

    /// <summary xml:lang = "ru">
    /// Предыдущие состояние запроса.
    /// </summary>
    public TransactionRequestState OldState { get; }
}
