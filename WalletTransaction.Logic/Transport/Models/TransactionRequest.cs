using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель запроса на проведение транзаций.
/// </summary>
public sealed class TransactionRequest
{
    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Коллекция транзакций, которые нужно выполнить в рамках запроса.
    /// </summary>
    [JsonProperty("transactions", Required = Required.Always)]
    public IReadOnlyCollection<Transaction> Transactions { get; set; } = default!;
}
