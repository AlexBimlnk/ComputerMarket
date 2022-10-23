namespace Market.Logic.Commands.Import;

/// <summary xml:lang = "ru">
/// Маркерный класс команд для сервиса импорта.
/// </summary>
public abstract class ImportCommand : CommandBase
{
    protected ImportCommand(CommandId id) : base(id)
    {
    }
}
