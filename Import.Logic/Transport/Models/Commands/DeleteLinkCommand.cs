using Import.Logic.Models;

using Newtonsoft.Json;

namespace Import.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для удаления связи.
/// </summary>
public sealed class DeleteLinkCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="DeleteLinkCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public DeleteLinkCommand(CommandType type, string id) : base(type, id) { }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("internal_id")]
    public long InternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    [JsonProperty("external_id")]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Имя провайдера.
    /// </summary>
    [JsonProperty("provider")]
    public Provider Provider { get; set; } = default!;
}
