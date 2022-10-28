using Newtonsoft.Json;

namespace Import.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для установки связи.
/// </summary>
public sealed class SetLinkCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public SetLinkCommand(CommandType type, string id) : base(type, id) { }

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
    /// Идентификатор провайдера.
    /// </summary>
    [JsonProperty("provider_id")]
    public long ProviderID { get; set; } = default!;
}