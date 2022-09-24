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
        SetLinkCommandParameters parameters)
        : base(id)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор на который устанавливается связь.
    /// </summary>
    public ExternalID ExternalID { get; }

    protected override Task ExecuteCoreAsync()
    {

    }
}