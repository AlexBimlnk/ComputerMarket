using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;

namespace Import.Logic.Transport.Receivers;
public sealed class HTTPPushReceiver
{
    private readonly ICommandFactory _commandFactory;
    private readonly IDeserializer<string, CommandParametersBase> _deserializer;

    public HTTPPushReceiver(
        ICommandFactory commandFactory,
        IDeserializer<string, CommandParametersBase> deserializer)
    {
        _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
    }

    public async Task ProcessAsync(string request)
    {
        var parameters = _deserializer.Deserialize(request);

        var command = _commandFactory.Create(parameters);

        await command.ExecuteAsync();
    }
}
