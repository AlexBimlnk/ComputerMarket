using General.Logic.Commands;
using General.Logic.Executables;
using General.Logic.Queries;

using Import.Logic.Abstractions;
using Import.Logic.Commands;

namespace Import.Logic.Queries;

/// <summary xml:lang="ru">
/// Результат выполнения комманды с вернувшимся значением.
/// </summary>
/// <typeparam name="TEntity" xml:lang="ru">
/// Тип данных который возвращется вместе с результатом команды.
/// </typeparam>
public sealed class QueryResult<TEntity> : IQueryResult<TEntity> where TEntity : class
{
    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="QueryResult{TEntity}"/>.
    /// </summary>
    /// <param name="id" xml:lang="ru">Идентификатор команды.</param>
    /// <param name="errorMessge" xml:lang="ru">Сообщенеи оь ошибке.</param>
    /// <param name="result" xml:lang="ru">Результат выполнения команды.</param>
    private QueryResult(ExecutableID id, string? errorMessge = null, TEntity? result = null)
    {
        Id = id;
        ErrorMessage = errorMessge;
        Result = result;
    }

    /// <inheritdoc/>
    public ExecutableID Id { get; }

    /// <inheritdoc/>
    public string? ErrorMessage { get; }

    /// <inheritdoc/>
    public TEntity? Result { get; }

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
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="errorMessage"/> или пустое или содержит только пробелы или <see langword="null"/>.
    /// </exception>
    public static QueryResult<TEntity> Fail(ExecutableID id, string errorMessage) =>
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
    /// Если один из параметров <see langword="null"/>.
    /// </exception>
    public static QueryResult<TEntity> Success(ExecutableID id, TEntity result) =>
        new(id ?? throw new ArgumentNullException(nameof(id)), result: result ?? throw new ArgumentNullException(nameof(id)));
}
