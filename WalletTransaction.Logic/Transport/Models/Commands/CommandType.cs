using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WalletTransaction.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Типы команд обрабатываемых сервисом банковский транзакций.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum CommandType
{
    /// <summary xml:lang = "ru">
    /// Созание нового запроса.
    /// </summary>
    CreateTransactionRequest,

    /// <summary xml:lang = "ru">
    /// Отмена запроса.
    /// </summary>
    CancelTransactionRequest,

    /// <summary xml:lang = "ru">
    /// Полный рассчет запроса.
    /// </summary>
    FinishTransactionRequest
}
