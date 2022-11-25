using General.Logic.Executables;

namespace General.Logic.Queries;

/// <summary xml:lang = "ru">
/// Описывает результат выполнения запроса.
/// </summary>
public interface IQueryResult : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Результат выполнения запроса.
    /// </summary>
    public object? Result { get; }
}
