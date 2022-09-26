using Newtonsoft.Json;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Внешние поставщики продуктов.
/// </summary>
public enum Provider
{
    /// <summary xml:lang = "ru">
    /// Рога и копыта.
    /// </summary>
    [JsonProperty("horns_and_hooves")]
    HornsAndHooves,
    /// <summary xml:lang = "ru">
    /// Иванов.
    /// </summary>
    [JsonProperty("ivanov")]
    Ivanov
}
