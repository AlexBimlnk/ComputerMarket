using General.Logic.Executables;
using General.Logic.Queries;

namespace Market.Logic.Queries;

/// <summary xml:lang = "ru">
/// Результат выполнения команды.
/// </summary>
public sealed class QueryResult<TResult> : IQueryResult<TResult>
{
    /// <summary xml:lang = "ru">
    /// Создает экземляр класса <see cref="IQueryResult{TResult}"/>.
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
    public QueryResult(ExecutableID id, TResult? result, string? errorMessge)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Result = result;
        ErrorMessage = errorMessge;
    }

    /// <inheritdoc/>
    public ExecutableID Id { get; }

    /// <inheritdoc/>
    public bool IsSuccess => ErrorMessage is null;

    /// <inheritdoc/>
    public string? ErrorMessage { get; }

    /// <inheritdoc/>
    public TResult? Result { get; }
}