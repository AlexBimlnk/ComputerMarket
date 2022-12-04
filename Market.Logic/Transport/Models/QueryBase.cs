using Market.Logic.Queries;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models;

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
    [JsonProperty("type", Required = Required.Always)]
    public QueryType Type { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public string Id { get; }
}
