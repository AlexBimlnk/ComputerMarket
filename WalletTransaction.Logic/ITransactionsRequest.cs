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
}
