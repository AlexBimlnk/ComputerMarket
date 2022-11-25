using Import.Logic.Transport.Models.Commands;

using Newtonsoft.Json;

namespace Import.Logic.Transport.Models.Queries;

/// <summary xml:lang = "ru">
/// Представляет базовую транспортную модель запроса.
/// </summary>
public class QueryBase
{
    /// <summary xml:lang = "ru">
    /// Создает объект типа <see cref="CommandBase"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип запроса.
    /// </param>
    protected QueryBase(QueryType type, string id)
    {
        Id = id;
        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Тип запроса.
    /// </summary>
    [JsonProperty("type")]
    public QueryType Type { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; }
}
