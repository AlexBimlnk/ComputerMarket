using General.Logic.Executables;
using General.Transport;

using Market.Logic.Models;
using Market.Logic.Queries;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Десериализатор результат выполнения команд.
/// </summary>
public sealed class ImportQueryResultDeserializer :
    IDeserializer<string, QueryResult<IReadOnlyCollection<Link>>>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="source"/> имеет некоректный формат.
    /// </exception>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если json структура в <paramref name="source"/> имееет неверную структуру.
    /// </exception>
    public QueryResult<IReadOnlyCollection<Link>> Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var result = _serializer.Deserialize<TQueryResult>(reader)!;

        if (result is null)
        {
            throw new InvalidOperationException("Give JSON file doesn't have correct structure.");
        }

        var links = result.Result?.Select(x
            => new Link(
                new(x.InternalID.Value),
                new(x.ExternalID.Value),
                new(x.ExternalID.ProviderID)))
            .ToList();

        return new QueryResult<IReadOnlyCollection<Link>>(
            new ExecutableID(result.Id.Value),
            links,
            result.ErrorMessage);
    }

    private sealed class TQueryResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public TQueryId Id { get; set; } = default!;

        [JsonProperty("errorMessage", Required = Required.AllowNull)]
        public string? ErrorMessage { get; set; }

        [JsonProperty("result", Required = Required.AllowNull)]
        public IReadOnlyCollection<TLink> Result { get; set; } = default!;
    }

    private sealed class TQueryId
    {
        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; } = default!;
    }

    private sealed class TLink
    {
        [JsonProperty("internalId", Required = Required.Always)]
        public TInternalId InternalID { get; set; } = default!;

        [JsonProperty("internalId", Required = Required.Always)]
        public TExternalId ExternalID { get; set; } = default!;
    }
    private sealed class TInternalId
    {
        [JsonProperty("value", Required = Required.Always)]
        public long Value { get; set; }
    }

    private sealed class TExternalId
    {
        [JsonProperty("value", Required = Required.Always)]
        public long Value { get; set; }

        [JsonProperty("provider", Required = Required.Always)]
        public long ProviderID { get; set; }
    }
}


