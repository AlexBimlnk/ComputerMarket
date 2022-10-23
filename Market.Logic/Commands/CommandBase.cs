namespace Market.Logic.Commands;

/// <summary xml:lang = "ru">
/// Абстрактная базовая команда.
/// </summary>
public abstract class CommandBase
{
    protected CommandBase(CommandId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public CommandId Id { get; }
}
