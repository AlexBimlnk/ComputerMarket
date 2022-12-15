using General.Transport;

using Market.Logic.Transport.Models;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Дессериализатор <see cref="TransportProduct"/>.
/// </summary>
public sealed class ProductDeserializer : IDeserializer<string, IReadOnlyCollection<TransportProduct>>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public IReadOnlyCollection<TransportProduct> Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var result = _serializer.Deserialize<IReadOnlyCollection<TransportProduct>>(reader)!;

        return result;
    }
}