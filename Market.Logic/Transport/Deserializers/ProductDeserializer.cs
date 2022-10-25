using General.Transport;
using Market.Logic.Transport.Models;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Дессериализатор <see cref="Product"/>.
/// </summary>
public sealed class ProductDeserializer : IDeserializer<string, IReadOnlyCollection<Product>>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public IReadOnlyCollection<Product> Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var result = _serializer.Deserialize<IReadOnlyCollection<Product>>(reader)!;

        return result;
    }
}