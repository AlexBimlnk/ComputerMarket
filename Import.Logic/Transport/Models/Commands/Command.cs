using Newtonsoft.Json;

namespace Import.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Представляет базовую транспортную модель команды.
/// </summary>
public abstract class Command
{
    /// <summary xml:lang = "ru">
    /// Создает объект типа <see cref="Command"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    protected Command(CommandType type, string id)
    {
        Id = id;
        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Тип команды.
    /// </summary>
    [JsonProperty("type")]
    public CommandType Type { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; }
}
