using Newtonsoft.Json;

namespace Market.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель продукта.
/// </summary>
public sealed class TransportProduct
{
    /// <summary xml:lang = "ru">
    /// Внешний индетификатор продукта.
    /// </summary>
    [JsonProperty("external_id", Required = Required.Always)]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Внутрений индетификатор продукта.
    /// </summary>
    [JsonProperty("internal_id", Required = Required.Always)]
    public long InternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Название поставщика.
    /// </summary>
    [JsonProperty("provider_id", Required = Required.Always)]
    public long ProviderID { get; set; }

    /// <summary xml:lang = "ru">
    /// Цена продукта.
    /// </summary>
    [JsonProperty("price", Required = Required.Always)]
    public decimal Price { get; set; }

    /// <summary xml:lang = "ru">
    /// Колличество продукта.
    /// </summary>
    [JsonProperty("quantity", Required = Required.Always)]
    public int Quantity { get; set; }
}