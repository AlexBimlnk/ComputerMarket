namespace Market.Logic.Commands;

/// <summary xml:lang = "ru">
/// Абстрактная базовая команда.
/// </summary>
public abstract class CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandBase"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected CommandBase(CommandId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public CommandId Id { get; }
}
