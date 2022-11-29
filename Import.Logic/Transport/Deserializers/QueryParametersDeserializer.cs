using General.Logic.Queries;
using General.Transport;

using Import.Logic.Queries;
using Import.Logic.Transport.Models.Queries;

using Newtonsoft.Json;

namespace Import.Logic.Transport.Deserializers;

using TGetLinksQuery = Models.Queries.GetLinksQuery;
using TQueryBase = Models.Queries.QueryBase;

/// <summary xml:lang = "ru">
/// Дессериализатор из <see cref="string"/> в <see cref="QueryParametersBase"/>.
/// </summary>
public sealed class QueryParametersDeserializer : IDeserializer<string, QueryParametersBase>
{
    private readonly JsonSerializer _serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
    {
        Converters = new[]
        {
            new QueryConverter()
        }
    });

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public QueryParametersBase Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var trasnportCommand = _serializer.Deserialize<TQueryBase>(reader);

        return trasnportCommand switch
        {
            TGetLinksQuery setLinkCommand => new GetLinksQueryParameters(
                new(setLinkCommand.Id)),

            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };
    }
}
