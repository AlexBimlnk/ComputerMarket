using Newtonsoft.Json;

namespace Market.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель продукта.
/// </summary>
public sealed class Product
{
    /// <summary xml:lang = "ru">
    /// Внешний индетификатор продукта.
    /// </summary>
    [JsonProperty("external_id")]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Внутрений индетификатор продукта.
    /// </summary>
    [JsonProperty("internal_id")]
    public long? InternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Название поставщика.
    /// </summary>
    [JsonProperty("provider_id")]
    public long? ProviderID { get; set; }

    /// <summary xml:lang = "ru">
    /// Цена продукта.
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary xml:lang = "ru">
    /// Колличество продукта.
    /// </summary>
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}