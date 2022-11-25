using General.Logic.Executables;

namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Базовый тип для параметров комманд.
/// </summary>
public abstract class CommandParametersBase : ExecutableParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandParametersBase"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected CommandParametersBase(ExecutableID id) : base(id)
    { }
}
