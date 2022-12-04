using General.Logic.Executables;

namespace Market.Logic.Commands.Import;

/// <summary xml:lang = "ru">
/// Маркерный класс команд для сервиса импорта.
/// </summary>
public abstract class WTCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="WTCommand"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    protected WTCommand(ExecutableID id, CommandType type)
        : base(id, type)
    {
    }
}
