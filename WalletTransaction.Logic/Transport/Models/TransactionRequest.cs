namespace WalletTransaction.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель запроса на проведение транзаций.
/// </summary>
public sealed class TransactionRequest
{
    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Коллекция транзакций, которые нужно выполнить в рамках запроса.
    /// </summary>
    public IReadOnlyCollection<Transaction> Transactions { get; set; } = default!;
}
