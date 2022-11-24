using General.Logic.Queries;
using General.Transport;

using Newtonsoft.Json;

namespace Import.Logic.Transport.Serializers;

/// <summary xml:lang = "ru">
/// Сериализатор результата запросов.
/// </summary>
public class QueryResultSerializer : ISerializer<IQueryResult, string>
{
    /// <inheritdoc/>
    public string Serialize(IQueryResult source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var transportResult = new TQueryResult()
        {
            Id = source.Id.Value,
            ErrorMessage = source.ErrorMessage,
            Result = source.Result
        };

        return JsonConvert.SerializeObject(transportResult);
    }

    private sealed class TQueryResult
    {
        [JsonProperty("id")]
        public string Id { get; set; } = default!;

        [JsonProperty("error_message")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("result")]
        public object? Result { get; set; }
    }
}
