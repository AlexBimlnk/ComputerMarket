using Import.Logic.Abstractions;
using Import.Logic.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Import.Logic.Transport.Serializers;

/// <summary xml:lang = "ru">
/// Сериализатор продуктов.
/// </summary>
public sealed class ProductsSerializer : ISerializer<IReadOnlyCollection<Product>, string>
{
    /// <inheritdoc/>
    public string Serialize(IReadOnlyCollection<Product> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var transportProducts = source.Select(x => new TransportProduct
            {
                ExternalID = x.ExternalID.Value,
                InternalID = x.InternalID.Value,
                Provider = x.ExternalID.Provider,
                Price = x.Price.Value,
                Quantity = x.Quantity
            })
            .ToList();

        return JsonConvert.SerializeObject(transportProducts, new StringEnumConverter());
    }

    private sealed class TransportProduct
    {
        [JsonProperty("external_id")]
        public long ExternalID { get; set; }

        [JsonProperty("internal_id")]
        public long? InternalID { get; set; }

        [JsonProperty("provider_name")]
        public Provider Provider { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
