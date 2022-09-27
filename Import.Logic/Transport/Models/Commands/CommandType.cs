using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Import.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Типы команд обрабатываемых сервисом импорта.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum CommandType
{
    /// <summary xml:lang = "ru">
    /// Установка связи.
    /// </summary>
    SetLink
}
