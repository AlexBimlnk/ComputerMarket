using Import.Logic.Abstractions;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда на установку связи.
/// </summary>
public sealed class SetLinkCommand : Command
{
    private SetLinkCommandParameters _parameters;

    public SetLinkCommand(
        CommandID id,
        SetLinkCommandParameters parameters) : base(id)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    protected override Task ExecuteCoreAsync() => throw new NotImplementedException();
}