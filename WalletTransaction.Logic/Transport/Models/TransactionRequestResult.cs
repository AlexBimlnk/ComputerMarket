using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Представляет транспортную модель результата запроса по обработке транзакций.
/// </summary>
public sealed class TransactionRequestResult
{
    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий на то, что запрос был отменён.
    /// </summary>
    [JsonProperty("is_cancelled", Required = Required.Always)]
    public bool IsCancelled { get; set; }

    /// <summary xml:lang = "ru">
    /// Последнее состояние в котором находился запрос.
    /// </summary>
    [JsonProperty("last_state", Required = Required.Always)]
    public TransactionRequestState LastState { get; set; }
}
