namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Описывает результат выполнения команды.
/// </summary>
public interface ICommandResult
{
    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public CommandID Id { get; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий является ли результат успешным.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary xml:lang = "ru">
    /// Сообщение содержащие описание ошибки.
    /// </summary>
    public string? ErrorMessage { get; }
}