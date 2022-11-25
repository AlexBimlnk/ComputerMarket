using General.Logic.Executables;

namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Описывает результат выполнения команды.
/// </summary>
public interface ICommandResult : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Флаг, указывающий является ли результат успешным.
    /// </summary>
    public bool IsSuccess { get; }
}