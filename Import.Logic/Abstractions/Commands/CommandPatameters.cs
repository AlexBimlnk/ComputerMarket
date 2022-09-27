using Import.Logic.Commands;

namespace Import.Logic.Abstractions.Commands;

/// <summary xml:lang = "ru">
/// Базовый тип параметров команд.
/// </summary>
public abstract class CommandParameters
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandParameters"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected CommandParameters(CommandID id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public CommandID Id { get; }
}
