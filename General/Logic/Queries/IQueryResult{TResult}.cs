using General.Logic.Executables;

namespace General.Logic.Queries;

/// <summary xml:lang = "ru">
/// Описывает результат выполнения запроса.
/// </summary>
/// <typeparam name="TResult">
/// Тип результата выполнения запроса.
/// </typeparam>
public interface IQueryResult<TResult> : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Результат выполнения запроса.
    /// </summary>
    public TResult? Result { get; }
}
