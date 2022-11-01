namespace Market.Logic.Commands.Import;

/// <summary xml:lang = "ru">
/// Маркерный класс команд для сервиса импорта.
/// </summary>
public abstract class ImportCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ImportCommand"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    protected ImportCommand(CommandId id, CommandType type)
        : base(id, type)
    {
    }
}
