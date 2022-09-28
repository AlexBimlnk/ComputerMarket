using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Внешние поставщики продуктов.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum Provider
{
    /// <summary xml:lang = "ru">
    /// Иванов.
    /// </summary>
    Ivanov = 1,

    /// <summary xml:lang = "ru">
    /// Рога и копыта.
    /// </summary>
    HornsAndHooves,
}
