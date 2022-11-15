using General.Transport;

using Market.Logic.Commands.Import;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Market.Logic.Transport.Serializers;

using CommandBase = Models.Commands.Import.CommandBase;
using TDeleteLinkCommand = Models.Commands.Import.DeleteLinkCommand;
using TSetLinkCommand = Models.Commands.Import.SetLinkCommand;

public sealed class ImportCommandSerializer : ISerializer<ImportCommand, string>
{
    public string Serialize(ImportCommand source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        CommandBase transportProducts = source switch
        {
            SetLinkCommand setLinkCommand => TSetLinkCommand.ToModel(setLinkCommand),
            DeleteLinkCommand deleteLinkCommand => TDeleteLinkCommand.ToModel(deleteLinkCommand),
            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };

        return JsonConvert.SerializeObject(transportProducts, new StringEnumConverter());
    }
}
