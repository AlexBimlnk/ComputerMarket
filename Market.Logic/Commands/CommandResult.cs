using General.Logic.Commands;

namespace Market.Logic.Commands;

/// <summary xml:lang = "ru">
/// Результат выполнения команды.
/// </summary>
public sealed class CommandResult : ICommandResult
{
    /// <summary>
    /// Создает экземляр класса <see cref="CommandResult"/>/
    /// </summary>
    /// <param name="id">Идентификатор команды.</param>
    /// <param name="errorMessge">Сообщение об ошибке.</param>
    /// <exception cref="ArgumentNullException">
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