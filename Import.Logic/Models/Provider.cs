using Newtonsoft.Json;

namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Внешние поставщики продуктов.
/// </summary>
public enum Provider
{
    /// <summary xml:lang = "ru">
    /// Иванов.
    /// </summary>
    [JsonProperty("ivanov")]
    Ivanov = 1,

    /// <summary xml:lang = "ru">
    /// Рога и копыта.
    /// </summary>
    [JsonProperty("horns_and_hooves")]
    HornsAndHooves
}
