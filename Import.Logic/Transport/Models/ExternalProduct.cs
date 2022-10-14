using Newtonsoft.Json;

namespace Import.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель продукта поставщика <see cref="Logic.Models.Provider.Ivanov"/>.
/// </summary>
public sealed class ExternalProduct
{
    /// <summary xml:lang = "ru">
    /// Идентификатор продукта.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string? Name { get; set; }

    /// <summary xml:lang = "ru">
    /// Цена.
    /// </summary>
    [JsonProperty("price", Required = Required.Always)]
    public decimal Price { get; set; }

    /// <summary xml:lang = "ru">
    /// Количество.
    /// </summary>
    [JsonProperty("quantity", Required = Required.Always)]
    public int Quantity { get; set; }

    /// <summary xml:lang = "ru">
    /// Описание продукта.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public IReadOnlyCollection<string>? Description { get; set; }
}