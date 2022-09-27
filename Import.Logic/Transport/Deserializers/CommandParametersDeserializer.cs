using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;
using Import.Logic.Transport.Models.Commands;

using Newtonsoft.Json;

namespace Import.Logic.Transport.Deserializers;

using TCommand = Models.Commands.Command;
using TSetLinkCommand = Models.Commands.SetLinkCommand;

/// <summary xml:lang = "ru">
/// Дессериализатор из <see cref="string"/> в <see cref="CommandParameters"/>.
/// </summary>
public sealed class CommandParametersDeserializer : IDeserializer<string, CommandParameters>
{
    private readonly JsonSerializer _serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
    {
        Converters = new[]
        {
            new CommandConverter()
        }
    });

    /// <inheritdoc/>
    public CommandParameters Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var trasnportCommand = _serializer.Deserialize<TCommand>(reader);

        return trasnportCommand switch
        {
            TSetLinkCommand setLinkCommand => new SetLinkCommandParameters(
                new(setLinkCommand.Id),
                new(setLinkCommand.InternalID),
                new(setLinkCommand.ExternalID, setLinkCommand.Provider)),

            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };
    }
}
