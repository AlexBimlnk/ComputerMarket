using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Market.Logic.Queries;

/// <summary xml:lang = "ru">
/// Типы запросов посылаемые во внешние сервисы.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum QueryType
{
    /// <summary xml:lang = "ru">
    ///  Получение связей.
    /// </summary>
    GetLinks,
}
