using Market.Logic.Models.WT;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель результата запроса на проведение транзакций.
/// </summary>
public sealed class TransactionRequestResult
{
    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    [JsonProperty("id")]
    public long TransactionRequestId { get; set; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий на то, что запрос был отменён.
    /// </summary>
    [JsonProperty("is_cancelled")]
    public bool IsCancelled { get; set; }

    /// <summary xml:lang = "ru">
    /// Состояние запроса.
    /// </summary>
    [JsonProperty("last_state")]
    public TransactionRequestState State { get; set; }
}
