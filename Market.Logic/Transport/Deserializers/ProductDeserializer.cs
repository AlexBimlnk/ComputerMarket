using General.Transport;

using Market.Logic.Models;
using Market.Logic.Transport.Models;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Дессериализатор <see cref="UpdateByProduct"/>.
/// </summary>
public sealed class ProductDeserializer : IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public IReadOnlyCollection<UpdateByProduct> Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var result = _serializer.Deserialize<IReadOnlyCollection<TransportProduct>>(reader)!
            .Select(x => new UpdateByProduct(
                new(x.ExternalID),
                new(x.InternalID),
                new(x.ProviderID),
                new(x.Price),
                x.Quantity))
            .ToList();

        return result;
    }
}