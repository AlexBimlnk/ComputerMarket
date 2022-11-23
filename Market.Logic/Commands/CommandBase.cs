using General.Logic.Executables;

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
    protected CommandBase(ExecutableID id, CommandType type)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));

        if (!Enum.IsDefined(typeof(CommandType), type))
            throw new ArgumentException("Given unknown command type", nameof(type));
        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public ExecutableID Id { get; }

    /// <summary xml:lang = "ru">
    /// Тип команды.
    /// </summary>
    public CommandType Type { get; }
}
