using General.Transport;

using Market.Logic.Commands.Import;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Market.Logic.Transport.Serializers;

using TSetLinkCommand = Transport.Models.Commands.Import.SetLinkCommand;

public sealed class ImportCommandSerializer : ISerializer<ImportCommand, string>
{
    public string Serialize(ImportCommand source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var transportProducts = source switch
        {
            SetLinkCommand setLinkCommand => TSetLinkCommand.ToModel(setLinkCommand),

            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };

        return JsonConvert.SerializeObject(transportProducts, new StringEnumConverter());
    }
}
