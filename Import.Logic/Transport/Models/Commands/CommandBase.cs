using Newtonsoft.Json;

namespace Import.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Представляет базовую транспортную модель команды.
/// </summary>
public abstract class CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает объект типа <see cref="CommandBase"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    protected CommandBase(CommandType type, string id)
    {
        Id = id;
        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Тип команды.
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public CommandType Type { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public string Id { get; }
}
