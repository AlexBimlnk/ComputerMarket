using Import.Logic.Abstractions;
using Import.Logic.Transport.Models;

using Newtonsoft.Json;

namespace Import.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Десериализатор внешних продуктов.
/// </summary>
public sealed class ExternalProductsDeserializer : IDeserializer<string, ExternalProduct[]>
{
    private readonly JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public ExternalProduct[] Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        return _serializer.Deserialize<ExternalProduct[]>(reader)!;
    }
}
