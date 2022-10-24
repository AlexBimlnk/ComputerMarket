using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Market.Logic.Commands;

/// <summary xml:lang = "ru">
/// Типы команд посылаемые во внешние сервисы.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum CommandType
{
    /// <summary xml:lang = "ru">
    /// Установка связи.
    /// </summary>
    SetLink,

    /// <summary xml:lang = "ru">
    /// Удаление связи.
    /// </summary>
    DeleteLink
}
