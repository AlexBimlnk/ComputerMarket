namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Результат выполнения команды.
/// </summary>
public sealed class CommandResult
{
    private CommandResult(CommandID id)
    {
        Id = id;
    }

    private CommandResult(CommandID id, string errorMessge) : this(id)
    {
        ErrorMessage = errorMessge;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public CommandID Id { get; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий является ли результат успешным.
    /// </summary>
    public bool IsSuccess => ErrorMessage is null;

    /// <summary xml:lang = "ru">
    /// Сообщение содержащие описание ошибки.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary xml:lang = "ru">
    /// Создает провальный результат команды.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <param name="errorMessage" xml:lang = "ru">
    /// Сообщение содержащие описание ошибки.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Результат неуспешной команды типа <see cref="CommandResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если идентификатор команды был <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lang = "ru"></exception>
    public static CommandResult Fail(CommandID id, string errorMessage) =>
        new(id ?? throw new ArgumentNullException(nameof(id)),
            string.IsNullOrWhiteSpace(errorMessage)
            ? throw new ArgumentException(
                "The error message can't be empty, null or has only whitespaces",
                nameof(errorMessage))
            : errorMessage);

    /// <summary xml:lang = "ru">
    /// Создает успешный результат команды.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Результат успешной команды типа <see cref="CommandResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если идентификатор команды был <see langword="null"/>.
    /// </exception>
    public static CommandResult Success(CommandID id) =>
        new(id ?? throw new ArgumentNullException(nameof(id)));
}