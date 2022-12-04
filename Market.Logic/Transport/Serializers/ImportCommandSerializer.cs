using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Transport.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Market.Logic.Transport.Serializers;

using TCommandBase = CommandBase;
using TDeleteLinkCommand = Models.Commands.Import.DeleteLinkCommand;
using TSetLinkCommand = Models.Commands.Import.SetLinkCommand;

/// <summary xml:lang = "ru">
/// Сериализатор команд для сервиса импорта.
/// </summary>
public sealed class ImportCommandSerializer : ISerializer<ImportCommand, string>
{
    /// <inheritdoc/>
    public string Serialize(ImportCommand source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TCommandBase transportCommand = source switch
        {
            SetLinkCommand setLinkCommand => TSetLinkCommand.ToModel(setLinkCommand),
            DeleteLinkCommand deleteLinkCommand => TDeleteLinkCommand.ToModel(deleteLinkCommand),
            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };

        return JsonConvert.SerializeObject(transportCommand, new StringEnumConverter());
    }
}
