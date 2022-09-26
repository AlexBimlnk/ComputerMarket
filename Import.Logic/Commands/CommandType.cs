using Newtonsoft.Json;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Типы команд обрабатываемых сервисом импорта.
/// </summary>
public enum CommandType
{
    /// <summary xml:lang = "ru">
    /// Установка связи.
    /// </summary>
    [JsonProperty("set_link")]
    SetLink
}
