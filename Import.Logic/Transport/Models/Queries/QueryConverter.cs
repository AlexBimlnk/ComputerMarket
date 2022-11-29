using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Import.Logic.Transport.Models.Queries;

/// <summary xml:lang = "ru">
/// Конвертер запросов.
/// </summary>
public sealed class QueryConverter : JsonConverter<QueryBase>
{
    /// <inheritdoc/>
    public override QueryBase? ReadJson(
        JsonReader reader,
        Type objectType,
        QueryBase? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var rawCommand = JObject.Load(reader);

        return rawCommand["type"]!.ToObject<QueryType>() switch
        {
            QueryType.GetLinks => rawCommand.ToObject<GetLinksQuery>(),
            var unknownCommandType =>
                throw new InvalidOperationException($"The unknown command type has been received '{unknownCommandType}'. ")
        };
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, QueryBase? value, JsonSerializer serializer) =>
        throw new NotSupportedException();
}
