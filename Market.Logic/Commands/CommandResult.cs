using General.Logic.Commands;

namespace Market.Logic.Commands;

/// <summary xml:lang = "ru">
/// Результат выполнения команды.
/// </summary>
public sealed class CommandResult : ICommandResult
{
    /// <summary xml:lang = "ru">
    /// Создает экземляр класса <see cref="CommandResult"/>/
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <param name="errorMessge" xml:lang = "ru">
    /// Сообщение об ошибке.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="id"/> - <see langword="null"/>.
    /// </exception>
    public CommandResult(CommandID id, string? errorMessge)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));

        ErrorMessage = errorMessge;
    }

    /// <inheritdoc/>
    public CommandID Id { get; }

    /// <inheritdoc/>
    public bool IsSuccess => ErrorMessage is null;

    /// <inheritdoc/>
    public string? ErrorMessage { get; }
}