using Newtonsoft.Json;

namespace Import.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Внутренняя транспортная модель продукта.
/// </summary>
public sealed class InternalProduct
{
    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    [JsonProperty("external_id")]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("internal_id")]
    public long? InternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    [JsonProperty("provider_name")]
    public string Provider { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Цена.
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary xml:lang = "ru">
    /// Количество.
    /// </summary>
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}