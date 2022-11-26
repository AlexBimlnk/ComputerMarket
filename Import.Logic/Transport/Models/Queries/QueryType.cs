using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Import.Logic.Transport.Models.Queries;

/// <summary xml:lang = "ru">
/// Типы запросов обрабатываемых сервисом импорта.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum QueryType
{
    /// <summary xml:lang = "ru">
    ///  Получение связей.
    /// </summary>
    GetLinks,
}
