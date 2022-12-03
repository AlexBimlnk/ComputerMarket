using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Market.Logic.Models.WT;

/// <summary xml:lang = "ru">
/// Описывает состояния, в которых может 
/// находится запрос на проведение транзакций.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum TransactionRequestState
{
    /// <summary xml:lang = "ru">
    /// Удерживается магазином.
    /// </summary>
    Held,

    /// <summary xml:lang = "ru">
    /// Оплата дошла до всех поставщиков.
    /// </summary>
    Finished,

    /// <summary xml:lang = "ru">
    /// Произведен возврат средств.
    /// </summary>
    Refunded,

    /// <summary xml:lang = "ru"> 
    /// Критическое состояние ошибки 
    /// во время исполнения транзакции.
    /// </summary>
    Aborted
}
