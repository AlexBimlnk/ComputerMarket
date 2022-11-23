using General.Logic.Executables;
using General.Logic.Queries;

using Import.Logic.Queries;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Абстрактная команда возвращающая результат.
/// </summary>
public abstract class QueryBase<TEntity> : IQuery
{
    protected object? Result { get; set; }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public abstract ExecutableID Id { get; }

    protected abstract Task ExecuteCoreAsync();

    /// <inheritdoc/>
    public async Task<IQueryResult> ExecuteAsync()
    {
        try
        {
            await ExecuteCoreAsync()
                .ConfigureAwait(false);

            return QueryResult<TEntity>.Success(Id, Result ?? throw new InvalidOperationException("Result of command execution is null"));
        }
        catch (InvalidOperationException ex)
        {
            return QueryResult<TEntity>.Fail(Id, ex.Message);
        }
    }
}
