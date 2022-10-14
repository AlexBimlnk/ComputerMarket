using Newtonsoft.Json;

namespace Import.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель продукта поставщика <see cref="Logic.Models.Provider.HornsAndHooves"/>.
/// </summary>
public sealed class HornsAndHoovesProduct
{
    /// <summary xml:lang = "ru">
    /// Идентификатор продукта.
    /// </summary>
    [JsonProperty("product_id", Required = Required.Always)]
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название.
    /// </summary>
    [JsonProperty("product_name", Required = Required.Always)]
    public string? Name { get; set; }

    /// <summary xml:lang = "ru">
    /// Цена.
    /// </summary>
    [JsonProperty("product_price", Required = Required.Always)]
    public decimal Price { get; set; }

    /// <summary xml:lang = "ru">
    /// Количество.
    /// </summary>
    [JsonProperty("product_quantity", Required = Required.Always)]
    public int Quantity { get; set; }

    /// <summary xml:lang = "ru">
    /// Описание продукта.
    /// </summary>
    [JsonProperty("product_description", Required = Required.Always)]
    public string Description { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Тип продукта.
    /// </summary>
    [JsonProperty("product_type", Required = Required.Always)]
    public string Type { get; set; } = default!;
}
