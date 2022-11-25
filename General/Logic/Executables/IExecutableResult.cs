namespace General.Logic.Executables;

/// <summary xml:lang = "ru">
/// Описвает результат исполяемой сущности.
/// </summary>
public interface IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Идентификатор сущности.
    /// </summary>
    public ExecutableID Id { get; }

    /// <summary xml:lang = "ru">
    /// Сообщение содержащие описание ошибки.
    /// </summary>
    public string? ErrorMessage { get; }
}
