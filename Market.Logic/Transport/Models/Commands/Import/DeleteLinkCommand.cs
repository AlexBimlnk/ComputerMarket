using Market.Logic.Commands;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models.Commands.Import;

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
    public DeleteLinkCommand(string id, CommandType type) : base(id, type) { }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    [JsonProperty("external_id", Required = Required.Always)]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Идентификатор провайдера.
    /// </summary>
    [JsonProperty("provider_id", Required = Required.Always)]
    public long ProviderID { get; set; }

    public static DeleteLinkCommand ToModel(Logic.Commands.Import.DeleteLinkCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new DeleteLinkCommand(command.Id.Value, command.Type)
        {
            ExternalID = command.ExternalItemId.Value,
            ProviderID = command.Provider.Key.Value
        };
    }
}