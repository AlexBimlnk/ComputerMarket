using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Import.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Конвертер команд.
/// </summary>
public sealed class CommandConverter : JsonConverter<CommandBase>
{
    /// <inheritdoc/>
    public override CommandBase? ReadJson(
        JsonReader reader,
        Type objectType,
        CommandBase? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var rawCommand = JObject.Load(reader);

        return rawCommand["type"]!.ToObject<CommandType>() switch
        {
            CommandType.SetLink => rawCommand.ToObject<SetLinkCommand>(),
            var unknownCommandType =>
                throw new InvalidOperationException($"The unknown command type has been received '{unknownCommandType}'. ")
        };
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, CommandBase? value, JsonSerializer serializer) =>
        throw new NotSupportedException();
}
